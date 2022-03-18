using Godot;

public enum NoteType
{
    ShortNote,
    LongNote,
    Mine
}

public struct Note 
{
    public int Column { get; private set; }
    public float Time { get; private set; }
    public float Length { get; private set; }
    public NoteType Type { get; private set; }
    public bool IsExist { get; set; }

    public Note(float time, int column, NoteType type, float length)
    {
        Column = column;
        Time = time;
        Length = length;
        Type = type;
        IsExist = true;
    }
}

public struct TimingPoint
{

}

public class GameLogic
{
    private TimeLogic _timeLogic;
    private NoteLogic _noteLogic;
    private InputLogic _inputLogic;

    private Note[] _notes;

    public float CurrentTime { get; private set; }
    public int NextExistingNote { get; private set; }
    public bool[] KeyState { get; private set; }

    public GameLogic(ref Note[] notes, TimeLogic timeLogic, NoteLogic noteLogic, InputLogic inputLogic)
    {
        _notes = notes;
        _timeLogic = timeLogic;
        _noteLogic = noteLogic;
        _inputLogic = inputLogic;

        CurrentTime = _timeLogic.CurrentTime;
        KeyState = _inputLogic.KeyState;
    }

    public void Process()
    {
        _noteLogic.CurrentTime = _timeLogic.CurrentTime;
        _noteLogic.NextExistingNote = NextExistingNote;

        _noteLogic.Process(ref _notes);
        GetNextExistingNote();
    }

    public void Input(InputEvent input)
    {
        if (input is InputEventKey keyEvent)
        {
            _inputLogic.Update(
                _timeLogic.CurrentTime,
                NextExistingNote
            );

            _inputLogic.Process(keyEvent, ref _notes);
            KeyState = _inputLogic.KeyState;
            GetNextExistingNote();
        }
    }

    public void GetNextExistingNote()
    {
        for (int i = NextExistingNote; i != _notes.GetLength(0); i++)
        {
            if (_notes[i].IsExist == true)
            {
                NextExistingNote = i;
                return;
            }
        }
    }
}