using Godot;

public class GameLogic
{
    private Playfield _playfield;

    private TimeLogic _timeLogic;
    private NoteLogic _noteLogic;
    private InputLogic _inputLogic;

    private Note[] _notes;

    public int NextExistingNote { get; private set; }
    public bool[] KeyState { get; private set; }

    private float chartEndTime = 0f;

    public GameLogic(ref Note[] notes, Playfield playfield, TimeLogic timeLogic, NoteLogic noteLogic, InputLogic inputLogic)
    {
        _notes = notes;
        _playfield = playfield;
        _timeLogic = timeLogic;
        _noteLogic = noteLogic;
        _inputLogic = inputLogic;

        KeyState = _inputLogic.KeyState;
        chartEndTime = _notes[_notes.GetLength(0) - 1].Time + 500f;
    }

    public void Process()
    {
        _noteLogic.CurrentTime = _timeLogic.CurrentTime;
        _noteLogic.NextExistingNote = NextExistingNote;

        _noteLogic.Process(ref _notes);
        GetNextExistingNote();
    }

    public void FixedProcess()
    {
        if (_timeLogic.CurrentTime > chartEndTime)
            _playfield.ChartEnded();
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