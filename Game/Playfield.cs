using Godot;

public class Playfield : Node2D
{
    private Chart _chart;
    private Skin _skin;

    private Conveyor _conveyor;
    private Info _info;
    private Audio _audio;

    private GameLogic _gameLogic;
    private TimeLogic _timeLogic;
    private ScoreSystem _scoreSystem;
    private NoteLogic _noteLogic;
    private InputLogic _inputLogic;

    private float _currentTime = -2_000f;
    private float _addTime = 1000;
    private bool _isPaused = false;

    public override void _Ready()
    {
        ChartGenerator cg = new ChartGenerator();
        _chart = new Chart(
            "Artist",
            "Title",
            "Expert",
            4,
            cg.DoMagic()
        );

        SkinLoader skinLoader = new SkinLoader();
        _skin = skinLoader.Build(_chart.inputMode, "Userdata/Skin/", "4k.json");

        int[] inputMap = new int[4] {
            (int)KeyList.A,
            (int)KeyList.S,
            (int)KeyList.K,
            (int)KeyList.L,
        };

        _info = GetNode<Info>("Info");
        _conveyor = GetNode<Conveyor>("Conveyor");

        _timeLogic = new TimeLogic(
            _audio,
            -2_000f,
            1f
        );
        
        _scoreSystem = new ScoreSystem(_info);
        _noteLogic = new NoteLogic(_scoreSystem);
        
        _inputLogic = new NearestInputLogic(
            _chart.inputMode, 
            _scoreSystem, 
            inputMap,
            155f
        );

        _gameLogic = new GameLogic(
            ref _chart.notes,
            _timeLogic,
            _noteLogic,
            _inputLogic
        );

        _conveyor.Construct(
            ref _chart.notes,
            _skin,
            _timeLogic,
            _gameLogic,
            1.3f
        );

        _info.Construct(_skin, _scoreSystem);
    }

    public override void _Process(float delta)
    {  
        _timeLogic.Process(delta);
        _gameLogic.Process();
    }

    public override void _Input(InputEvent input)
    {
        _gameLogic.Input(input);

        if (Input.IsActionJustPressed("restart_chart"))
            GetTree().ReloadCurrentScene();

        if (Input.IsActionJustPressed("pause"))
        {
            _timeLogic.SetPause();
        }
    }
}