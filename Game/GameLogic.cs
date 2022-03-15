using Godot;

public struct Note 
{
    public int Column { get; private set; }
    public float Time { get; private set; }
    public float Length { get; private set; }
    public bool IsLongNote { get; private set; }
    public bool IsExist { get; set; }

    public Note(float time, int column, bool isLongNote, float length)
    {
        Column = column;
        Time = time;
        Length = length;
        IsLongNote = isLongNote;
        IsExist = true;
    }
}

public struct Chart
{
    public string Artist { get; private set; }
    public string Title { get; private set; }
    public string Difficulty { get; private set; }
    public int InputMode { get; private set; }
    public Note[] Notes;

    public Chart(string artist, string title, string difficulty, int inputMode, Note[] notes)
    {
        this.Artist = artist;
        this.Title = title;
        this.Difficulty = difficulty;
        this.InputMode = inputMode;
        this.Notes = notes;
    }
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