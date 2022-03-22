using Godot;

public enum InputLogicType
{
    Earliest,
    Nearest
}

public abstract class InputLogic
{
    protected int[] _inputMap;
    public bool[] KeyState { get; private set; }

    public float CurrentTime { get; private set; }
    public int NextExistingNote { get; private set; }

    protected float _hitWindow;

    protected ScoreSystem _scoreSystem;

    public void Construct(int inputMode, ScoreSystem scoreSystem, float hitWindow, int[] inputMap)
    {
        _scoreSystem = scoreSystem;
        _hitWindow = hitWindow;
        _inputMap = inputMap;
        KeyState = new bool[inputMode];
    }

    public void ChangeKeyState(int key, bool state)
    {
        KeyState[key] = state;
    }

    public void Update(float time, int nextExistingNote)
    {
        CurrentTime = time;
        NextExistingNote = nextExistingNote;
    }

    public void Process(InputEventKey keyEvent, ref Note[] notes)
    {
        if (keyEvent.Pressed)
        {
            int column = 0;
            foreach (int key in _inputMap)
            {
                if (keyEvent.Scancode == key)
                {
                    if (KeyState[column] == true)
                        return; // Спасёт от даблсетаперов, и хуйни как при зажатии кнопки при печати, но самое по себе решение тупое блять как и я
                    
                    ChangeKeyState(column, true);
                    ProcessPress(column, CurrentTime, ref notes);
                    return;
                }
                column++;
            }
        }

        if (!keyEvent.Pressed) // release
        {
            int column = 0;
            foreach (int key in _inputMap)
            {
                if (keyEvent.Scancode == key)
                {
                    ChangeKeyState(column, false);
                    return;
                }
                column++;
            }
        }
    
    }

    public abstract void ProcessPress(int column, float time, ref Note[] note);
}

public class EarlyInputLogic : InputLogic
{
    // Count only the earliest existing note
    public EarlyInputLogic(int inputMode, ScoreSystem scoreSystem, int[] inputMap, float hitWindow)
    {
        Construct(
            inputMode,
            scoreSystem,
            hitWindow,
            inputMap
        );
    }

    public override void ProcessPress(int column, float time, ref Note[] notes)
    {
        float deltaTime = 0;
        float earlyTime = 0;
        float lateTime = 0;

        for (int i = NextExistingNote; i != notes.GetLength(0) ; i++)
        {
            ref Note note = ref notes[i];

            if (!note.IsExist)
                continue;
            
            if (note.Column != column)
                continue;

            earlyTime = note.Time - _hitWindow;
            lateTime = note.Time + _hitWindow;

            if (time < earlyTime)
                return;    

            if (time > earlyTime && time < lateTime)
            {
                deltaTime = note.Time - time;
                _scoreSystem.ProcessHit(deltaTime);
                note.IsExist = false;
                return;
            }
        }
    }
}

public class NearestInputLogic : InputLogic
{
    // Looking for an early and late note relative to the current time
    // And we count the nearest of these two
    public NearestInputLogic(int inputMode, ScoreSystem scoreSystem, int[] inputMap, float hitWindow)
    {
        Construct(
            inputMode,
            scoreSystem,
            hitWindow,
            inputMap
        );
    }

    public override void ProcessPress(int column, float time, ref Note[] notes)
    {   
        int earlyNoteIndex = GetEarlyNoteIndex(column, time, ref notes);
        int lateNoteIndex = GetLateNoteIndex(column, time, ref notes);
         
        if (earlyNoteIndex != -1 && lateNoteIndex != -1)
        {   // If early and late note exists
            if (notes[earlyNoteIndex].Time - time < time - notes[lateNoteIndex].Time)
                CountNote(notes, earlyNoteIndex, time);
            else
                CountNote(notes, lateNoteIndex, time);
            return;
        }

        if (earlyNoteIndex == -1 && lateNoteIndex != -1)
        {   // If late note exists, but early not (start of the chart)
            CountNote(notes, lateNoteIndex, time);
            return;
        }

        if (earlyNoteIndex != -1 && lateNoteIndex == -1)
        {   // If early note exists, but late not (end of the chart)
            CountNote(notes, earlyNoteIndex, time);
            return;
        }
    }

    public int GetLateNoteIndex(int column, float time, ref Note[] notes)
    {
        int index = -1; 
        for (int i = NextExistingNote; i != notes.GetLength(0); i++)
        {
            Note note = notes[i];

            if (!note.IsExist)
                continue;
            
            if (note.Column != column)
                continue;

            if (note.Time < time)
            {
                index = i;
                continue;
            }
            else
                return index;
        }
        return index;
    }

    public int GetEarlyNoteIndex(int column, float time, ref Note[] notes)
    {
        for (int i = NextExistingNote; i != notes.GetLength(0); i++)
        {
            Note note = notes[i];

            if (!note.IsExist)
                continue;

            if (note.Column != column)
                continue;

            if (note.Time > time)
                return i;
        }
        return -1;
    }

    public void CountNote(Note[] notes, int noteIndex, float time)
    {
        float earlyTime = notes[noteIndex].Time - _hitWindow;
        float lateTime = notes[noteIndex].Time + _hitWindow;
        float deltaTime = 0;

        if (time > earlyTime && time < lateTime)
        {
            deltaTime = notes[noteIndex].Time - time;
            _scoreSystem.ProcessHit(deltaTime);
            notes[noteIndex].IsExist = false;
            return;
        }
    }
}