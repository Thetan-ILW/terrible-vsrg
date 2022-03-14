using Godot;

public struct Note 
{
    public float time;
    public int column;
    public bool isLongNote;
    public float length;
    public bool isExist;

    public Note(float time, int column, bool isLongNote, float length)
    {
        this.time = time;
        this.column = column;
        this.isLongNote = isLongNote;
        this.length = length;
        isExist = true;
    }
}

public struct Chart
{
    public string artist;
    public string title;
    public string difficulty;
    public int inputMode; // сделай их константами браток
    public Note[] notes;

    public Chart(string artist, string title, string difficulty, int inputMode, Note[] notes)
    {
        this.artist = artist;
        this.title = title;
        this.difficulty = difficulty;
        this.inputMode = inputMode;
        this.notes = notes;
    }
}

public class GameLogic
{
    private TimeLogic _timeLogic;
    private NoteLogic _noteLogic;
    private InputLogic _inputLogic;

    private Note[] _notes;

    public float CurrentTime = 0;
    public int NextExistingNote = 0;
    public bool[] KeyState;

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
            _inputLogic.CurrentTime = _timeLogic.CurrentTime;
            _inputLogic.NextExistingNote = NextExistingNote;

            _inputLogic.Process(keyEvent, ref _notes);
            KeyState = _inputLogic.KeyState;
            GetNextExistingNote();
        }
    }

    public void GetNextExistingNote()
    {
        for (int i = NextExistingNote; i != _notes.GetLength(0); i++)
        {
            if (_notes[i].isExist == true)
            {
                NextExistingNote = i;
                return;
            }
        }
    }
}