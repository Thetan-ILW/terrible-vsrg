using Godot;
using System;

public abstract class InputLogic // Реагирует на нажатия игрока
{
    protected int[] _inputMap;
    public bool[] KeyState;

    public float CurrentTime = 0;
    public int NextExistingNote = 0;

    protected float _hitWindow = 155f;

    protected ScoreSystem _scoreSystem;

    public void ChangeKeyState(int key, bool state)
    {
        KeyState[key] = state;
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

        if (!keyEvent.Pressed) // Тоесть release
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
    // Считаем самую раннюю ноту попадающую под зону попадания
    public EarlyInputLogic(int inputMode, ScoreSystem scoreSystem, int[] inputMap, float hitWindow)
    {
        _inputMap = inputMap;
        _scoreSystem = scoreSystem;
        KeyState = new bool[inputMode];
        _hitWindow = hitWindow;
    }

    public override void ProcessPress(int column, float time, ref Note[] notes)
    {
        float deltaTime = 0;
        float earlyTime = 0;
        float lateTime = 0;

        for (int i = NextExistingNote; i != notes.GetLength(0) ; i++)
        {
            ref Note note = ref notes[i];

            if (!note.isExist)
                continue;
            
            if (note.column != column)
                continue;

            earlyTime = note.time - _hitWindow;
            lateTime = note.time + _hitWindow;

            if (time < earlyTime)
                return;    

            if (time > earlyTime && time < lateTime)
            {
                deltaTime = note.time - time;
                _scoreSystem.ProcessHit(deltaTime);
                note.isExist = false;
                return;
            }
        }
    }
}

public class NearestInputLogic : InputLogic
{
    // Ищем раннюю и позднюю ноту относительно текущего времени
    // И считаем только ту которая ближе к текущему времени
    public NearestInputLogic(int inputMode, ScoreSystem scoreSystem, int[] inputMap, float hitWindow)
    {
        this._inputMap = inputMap;
        this._scoreSystem = scoreSystem;
        KeyState = new bool[inputMode];
        _hitWindow = hitWindow;
    }

    public override void ProcessPress(int column, float time, ref Note[] notes)
    {   
        int earlyNoteIndex = GetEarlyNoteIndex(column, time, ref notes);
        int lateNoteIndex = GetLateNoteIndex(column, time, ref notes);
         
        if (earlyNoteIndex != -1 && lateNoteIndex != -1)
        {   // If early and late note exists
            if (notes[earlyNoteIndex].time - time < time - notes[lateNoteIndex].time)
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

            if (!note.isExist)
                continue;
            
            if (note.column != column)
                continue;

            if (note.time < time)
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

            if (!note.isExist)
                continue;

            if (note.column != column)
                continue;

            if (note.time > time)
                return i;
        }
        return -1;
    }

    public void CountNote(Note[] notes, int noteIndex, float time)
    {
        float earlyTime = notes[noteIndex].time - _hitWindow;
        float lateTime = notes[noteIndex].time + _hitWindow;
        float deltaTime = 0;

        if (time > earlyTime && time < lateTime)
        {
            deltaTime = notes[noteIndex].time - time;
            _scoreSystem.ProcessHit(deltaTime);
            notes[noteIndex].isExist = false;
            return;
        }
    }
}