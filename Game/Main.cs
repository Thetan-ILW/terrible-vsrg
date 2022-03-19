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

    private Settings _settings;

    private ScreenBuilder _screenBuilder;
    private SongSelect _songSelect;
    private Playfield _playfield;
    private ResultScreen _resultScreen;

    Screen _currentScreen;

    public override void _Ready()
    {
        SettingsLoader settingsLoader = new SettingsLoader();
        _screenBuilder = new ScreenBuilder();
        _settings = settingsLoader.GetSettings();
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

        _songSelect = _screenBuilder.GetSongSelect(this);
        AddChild(_songSelect);
        _currentScreen = Screen.SongSelect;
    }

    public void StartChart(string chartPath)
    {
        RemoveChild(_songSelect);
        _playfield = _screenBuilder.GetPlayfield(this, chartPath);
        AddChild(_playfield);
        _currentScreen = Screen.Playfield;
    }

    public void SetToResultScreen(ScoreSystem scoreSystem)
    {
        RemoveChild(_playfield);
        _resultScreen = _screenBuilder.GetResultScreen(this, scoreSystem);
        AddChild(_resultScreen);
        _currentScreen = Screen.Result;
    }
}
