using Godot;

public class ScreenBuilder
{
    public SongSelect GetSongSelect(Main main)
    {
        PackedScene songSelectScene = GD.Load<PackedScene>("res://Nodes/SongSelect.tscn");
        SongSelect songSelect = songSelectScene.Instance<SongSelect>();
        songSelect.Init(main);
        return songSelect;
    }

    public Playfield GetPlayfield(Main main, string chartPath)
    {
        SettingsLoader settingsLoader = new SettingsLoader();
        Settings settings = settingsLoader.GetSettings();
    
        string directoryName = chartPath.Substring(
            0, 
            chartPath.LastIndexOf("/") + 1
        );
        string chartName = chartPath.Substring(directoryName.Length);

        OsuParser _osuParser = new OsuParser();
        Chart chart = _osuParser.GetChart(directoryName + chartName);

        SkinLoader skinLoader = new SkinLoader();
        Skin skin = skinLoader.Build(chart.InputMode, "Userdata/Skin/");

        Modifiers modifiers = new Modifiers(true, 1f);

        Audio audio = new Audio(modifiers.TimeRate, settings.MusicVolume);

        audio.SetPlaybackSpeed(modifiers.TimeRate);
        audio.SetAudio(directoryName + chart.AudioPath);

        return new Playfield(main, skin, chart, audio, modifiers, settings);
    }

    public ResultScreen GetResultScreen(Main main, ScoreSystem scoreSystem)
    {
        return new ResultScreen(main, scoreSystem);
    }
}