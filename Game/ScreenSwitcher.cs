using Godot;

public class ScreenSwitcher
{
    private Main _main;
    private Audio _audio;

    private Playfield _playfield;
    private SongSelect _songSelect;

    public ScreenSwitcher(Main main, Audio audio)
    {
        _main = main;
        _audio = audio;
    }

    public void SetToMainMenu()
    {
        PackedScene songSelect = GD.Load<PackedScene>("res://Nodes/SongSelect.tscn");
        _songSelect = songSelect.Instance<SongSelect>();
        _songSelect.Init(_main);
        _main.AddChild(_songSelect);
    }

    public void SetToPlayfield(string path)
    {
        SettingsLoader settingsLoader = new SettingsLoader();
        Settings settings = settingsLoader.GetSettings();
    
        string directoryName = path.Substring(
            0, 
            path.LastIndexOf("/") + 1
        );
        string chartName = path.Substring(directoryName.Length);

        OsuParser _osuParser = new OsuParser();
        Chart chart = _osuParser.GetChart(directoryName + chartName);

        SkinLoader skinLoader = new SkinLoader();
        Skin skin = skinLoader.Build(chart.InputMode, "Userdata/Skin/", "4k.json");

        Modifiers modifiers = new Modifiers(true, 1f);

        _audio.SetPlaybackSpeed(modifiers.TimeRate);
        _audio.SetAudio(directoryName + chart.AudioPath);

        _playfield = new Playfield(skin, chart, _audio, modifiers, settings);

        _songSelect.Visible = false;
        _main.AddChild(_playfield);
    }
}