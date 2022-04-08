using Godot;
using System;

public class Main : Node2D
{
    enum Screen
    {
        SongSelect,
        Playfield,
        Result
    }

    private SettingsManager _settingsManager;
    private Settings _settings;

    private ScreenBuilder _screenBuilder;
    private SongSelect _songSelect;
    private Playfield _playfield;
    private ResultScreen _resultScreen;
    private Screen _currentScreen;

    private AnimationPlayer _animation;
    private SettingsMenu _settingsMenu;

    public override void _Ready()
    {
        _settingsManager = new SettingsManager();
        _screenBuilder = new ScreenBuilder();
        _settings = _settingsManager.Settings;

        _animation = GetNode<AnimationPlayer>("CanvasUi/AnimationPlayer");
        _songSelect = GetNode<SongSelect>("CanvasUi/SongSelect");
        _songSelect.Init(this);
        _settingsMenu = GetNode<SettingsMenu>("CanvasUi/SettingsMenu");
        _settingsMenu.Init(this, _settingsManager.Settings);

        SetToSongSelect();
    }

    public void SetToSongSelect()
    {
        if (_currentScreen == Screen.Playfield)
        {
            RemoveChild(_playfield);
            _playfield = null;
        }

        if (_currentScreen == Screen.Result)
            RemoveChild(_resultScreen);

        _currentScreen = Screen.SongSelect;
        _songSelect.Visible = true;
        Input.SetMouseMode(Input.MouseMode.Visible);
    }

    public void StartChart(string chartPath, float timeRate)
    {
        _playfield = _screenBuilder.GetPlayfield(this, _settings, chartPath, timeRate);
        AddChild(_playfield);
        _songSelect.Visible = false;
        GetTree().Root.Connect("size_changed", _playfield, nameof(_playfield.SizeChanged));
        _currentScreen = Screen.Playfield;
        Input.SetMouseMode(Input.MouseMode.Hidden);
    }

    public void SetToResultScreen(Chart chart, ScoreSystem scoreSystem)
    {
        RemoveChild(_playfield);
        _resultScreen = _screenBuilder.GetResultScreen(this, chart, scoreSystem);
        AddChild(_resultScreen);
        _currentScreen = Screen.Result;
        Input.SetMouseMode(Input.MouseMode.Visible);
    }

    public void ShowSettings() => _animation.Play("ShowSettings");
    public void HideSettings() 
    {
        _settingsManager.SaveFile();
        _animation.Play("HideSettings");
    }

    public void Quit() => GetTree().Quit();
}
