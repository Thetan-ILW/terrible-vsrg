using Godot;
using System;

public class Main : Node2D
{
    private Settings _settings;
    private Audio _audio;
    private ScreenSwitcher _screenSwitcher;

    public override void _Ready()
    {
        SettingsLoader settingsLoader = new SettingsLoader();
        Audio _audio = new Audio();
        _screenSwitcher = new ScreenSwitcher(this, _audio);
        _settings = settingsLoader.GetSettings();
        _audio = new Audio(1f, _settings.MusicVolume);

        SetToMainMenu();
    }

    public void SetToMainMenu()
    {
        _screenSwitcher.SetToMainMenu();
    }

    public void StartChart(string chartPath)
    {
        _screenSwitcher.SetToPlayfield(chartPath);
    }
}
