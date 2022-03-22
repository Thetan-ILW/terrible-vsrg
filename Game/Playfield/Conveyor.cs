using Godot;

public enum ConveyorDrawType
{
    RealTime,
    FixedFps
}

public abstract class Conveyor : Node2D
{
    private Skin _skin;
    private Note[] _notes; // тут ссылка на ноты если чё
    protected TimeLogic _timeLogic;
    protected GameLogic _gameLogic;

    protected DrawableNotes _drawableNotes;
    protected DrawableKeys _drawableKeys;

    protected float _currentTime = 0;
    protected int _nextExistingNote = 0;
    protected bool[] _keyState;

    public float ScrollSpeed = 0;

    protected void Constuct(ref Note[] notes, Skin skin, TimeLogic timeLogic, GameLogic gameLogic, float scrollSpeed)
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

    public override void _Draw()
    {
        _drawableNotes.Draw(this);
        _drawableKeys.Draw(this);
    }
}

public class RealTimeConveyor : Conveyor
{   // Draw notes every frame
    public RealTimeConveyor(ref Note[] notes, Skin skin, TimeLogic timeLogic, GameLogic gameLogic, float scrollSpeed)
    {
        Constuct(ref notes, skin, timeLogic, gameLogic, scrollSpeed);
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
}

public class FixedFpsConveyor : Conveyor
{   // Draw notes (physics_fps) times per second
    public FixedFpsConveyor(ref Note[] notes, Skin skin, TimeLogic timeLogic, GameLogic gameLogic, float scrollSpeed)
    {
        Constuct(ref notes, skin, timeLogic, gameLogic, scrollSpeed);
    }

    public override void _PhysicsProcess(float delta)
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
}