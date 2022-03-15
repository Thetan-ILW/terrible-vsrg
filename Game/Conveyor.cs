using Godot;

public class Conveyor : Node2D
{
    private Skin _skin;
    private Note[] _notes; // тут ссылка на ноты если чё
    private TimeLogic _timeLogic;
    private GameLogic _gameLogic;

    private DrawableNotes _drawableNotes;
    private DrawableKeys _drawableKeys;
    private DrawablePressedKeys _drawablePressedKeys;

    private float _currentTime = 0;
    private int _nextExistingNote = 0;
    private bool[] _keyState;

    public float ScrollSpeed = 0;

    public void Construct(ref Note[] notes, Skin skin, TimeLogic timeLogic, GameLogic gameLogic, float scrollSpeed)
    {
        _notes = notes;
        _skin = skin;
        _timeLogic = timeLogic;
        _gameLogic = gameLogic;

        _drawableNotes = new DrawableNotes();
        _drawableKeys = new DrawableKeys();
        _drawablePressedKeys = new DrawablePressedKeys();

        _keyState = _gameLogic.KeyState;

        ScrollSpeed = scrollSpeed;
    }

    public override void _Process(float delta)
    {
        _currentTime = _timeLogic.CurrentTime;
        _nextExistingNote = _gameLogic.NextExistingNote;
        _keyState = _gameLogic.KeyState;
        Update();
    }

    public override void _Draw()
    {
        _drawableNotes.Draw(
            this,
            _notes,
            _skin,
            _nextExistingNote,
            _currentTime,
            ScrollSpeed
        );

        _drawableKeys.Draw(
            this,
            _skin,
            _keyState
        );

        _drawablePressedKeys.Draw(
            this,
            _skin,
            _keyState
        );
    }
}