using Godot;

public class Conveyor : Node2D
{
    private Skin _skin;
    private Note[] _notes; // тут ссылка на ноты если чё
    private TimeLogic _timeLogic;
    private GameLogic _gameLogic;

    private DrawableNotes _drawableNotes;
    private DrawableKeys _drawableKeys;

    private float _currentTime = 0;
    private int _nextExistingNote = 0;
    private bool[] _keyState;

    public float ScrollSpeed = 0;

    public Conveyor(){}
    public Conveyor(ref Note[] notes, Skin skin, TimeLogic timeLogic, GameLogic gameLogic, float scrollSpeed)
    {
        _notes = notes;
        _skin = skin;
        _timeLogic = timeLogic;
        _gameLogic = gameLogic;
        _keyState = _gameLogic.KeyState;
        ScrollSpeed = scrollSpeed;

        _drawableNotes = new DrawableNotes(
            ref _notes,
            _skin
        );

        _drawableKeys = new DrawableKeys(
            _skin
        );

        _drawableNotes.Update(
            _nextExistingNote,
            _currentTime,
            ScrollSpeed
        );

        _drawableKeys.Update(
            _keyState
        );

    }

    public override void _Process(float delta)
    {
        _currentTime = _timeLogic.CurrentTime;
        _nextExistingNote = _gameLogic.NextExistingNote;
        _keyState = _gameLogic.KeyState;

        _drawableNotes.Update(
            _nextExistingNote,
            _currentTime,
            ScrollSpeed
        );

        _drawableKeys.Update(
            _keyState
        );

        Update();
    }

    public override void _Draw()
    {
        _drawableNotes.Draw(this);
        _drawableKeys.Draw(this);
    }
}