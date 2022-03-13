using Godot;
using System;
using System.Diagnostics;

public class Playfield : Node2D
{
    private Chart _chart;
    private Skin _skin;

    private Conveyor _conveyor;
    private Info _info;
    private Audio _audio;

    private GameLogic _gameLogic;
    private ScoreSystem _scoreSystem;
    private NoteLogic _noteLogic;
    private InputLogic _inputLogic;

    private float _currentTime = -2_000f;

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

        SkinFabric skinFabric = new SkinFabric();
        _skin = skinFabric.Build(_chart.inputMode, "Userdata/Skin/", "4k.json");

        int[] inputMap = new int[4] {
            (int)KeyList.A,
            (int)KeyList.S,
            (int)KeyList.K,
            (int)KeyList.L,
        };

        _info = GetNode<Info>("Info");
        _conveyor = GetNode<Conveyor>("Conveyor");

        _scoreSystem = new ScoreSystem(_info);
        _noteLogic = new NoteLogic(_scoreSystem);
        _inputLogic = new EarlyInputLogic(_chart.inputMode, _scoreSystem, inputMap);

        _gameLogic = new GameLogic(
            _noteLogic,
            _inputLogic,
            ref _chart.notes
        );

        _conveyor.Construct(ref _chart.notes, _skin, _inputLogic, 1.3f);
        _info.Construct(_skin, _scoreSystem);
    }

    public override void _Process(float delta)
    {  
        _currentTime += (delta * 1000);

        _gameLogic.CurrentTime = _currentTime;
        _gameLogic.Process();

        _conveyor.CurrentTime = _currentTime;
        _conveyor.NextExistingNote = _gameLogic.NextExistingNote;
    }

    public override void _Input(InputEvent input)
    {
        _gameLogic.Input(input);
        if (Input.IsActionJustPressed("restart_chart"))
            GetTree().ReloadCurrentScene();
    }
}