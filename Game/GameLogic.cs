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
    public Note[] note;

    public Chart(string artist, string title, string difficulty, int inputMode, Note[] note)
    {
        this.artist = artist;
        this.title = title;
        this.difficulty = difficulty;
        this.inputMode = inputMode;
        this.note = note;
    }
}

public class GameLogic
{
    private NoteLogic _noteLogic;
    private InputLogic _inputLogic;

    private Note[] _note;

    public float CurrentTime = 0;
    public int NextExistingNote = 0;

    public GameLogic(NoteLogic noteLogic, InputLogic inputLogic, ref Note[] note)
    {
        _noteLogic = noteLogic;
        _inputLogic = inputLogic;
        _note = note;
    }

    public void Process()
    {
        _noteLogic.CurrentTime = CurrentTime;
        _noteLogic.NextExistingNote = NextExistingNote;

        _noteLogic.Process(ref _note);
        GetNextExistingNote();
    }

    public void Input(InputEvent input)
    {
        if (input is InputEventKey keyEvent)
        {
            _inputLogic.CurrentTime = CurrentTime;
            _inputLogic.NextExistingNote = NextExistingNote;

            _inputLogic.Process(keyEvent, ref _note);
            GetNextExistingNote();
        }
    }

    public void GetNextExistingNote()
    {
        for (int i = NextExistingNote; i != _note.GetLength(0); i++)
        {
            if (_note[i].isExist == true)
            {
                NextExistingNote = i;
                return;
            }
        }
    }
}