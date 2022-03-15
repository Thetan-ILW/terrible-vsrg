using Godot;

public class Conveyor : Node2D
{
    private Skin _skin;
    private Note[] _notes; // тут ссылка на ноты если чё
    private TimeLogic _timeLogic;
    private GameLogic _gameLogic;

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
        // Notes
        // Начинаем рисовать с самой ранней существующей ноты 
        for (int i = _nextExistingNote; i != _notes.GetLength(0) ; i++)
        {
            Note note = _notes[i];
            // Считаем для каждой ноты из массива нот положение на экране
            float noteYPosition = (-note.Time + _currentTime + (_skin.Position.y / ScrollSpeed)) * ScrollSpeed;

            // Прерываем рисовку нот если хоть одна нота уже за экраном
            // Потому что после неё уже все ноты будут за границей видимости
            if (noteYPosition < -_skin.NoteSize.y)
                break; 

            if (note.IsExist != true)
                continue;
                     
            DrawTexture(
                _skin.NoteImage[note.Column],
                new Vector2(
                    _skin.Position.x + (note.Column * _skin.NoteSize.x),
                    noteYPosition)
            );
        }

        // Keys
        for (int i = 0; i != _skin.InputMode; i++)
        {
            if (_keyState[i] != false)
                continue;

            DrawTexture(
                _skin.KeyImage[i],
                new Vector2(
                    _skin.Position.x + (i * _skin.NoteSize.x),
                    _skin.Position.y
                )
            );
        }

        //Pressed keys
        for (int i = 0; i != _skin.InputMode; i++)
        {
            if (_keyState[i] != true)
                continue;
            
            DrawTexture(
                _skin.KeyPressedImage[i],
                new Vector2(
                    _skin.Position.x + (i * _skin.NoteSize.x),
                    _skin.Position.y
                )
            );
        }
    }
}