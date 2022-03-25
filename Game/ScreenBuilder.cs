using Godot;

public class ScreenBuilder
{
    public SongSelect GetSongSelect(Main main)
    {
        PackedScene songSelectScene = GD.Load<PackedScene>("res://Screens/SongSelect.tscn");
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

        float timeRate = 1f;

        Audio audio = new Audio(timeRate, settings.MusicVolume);

        audio.SetPlaybackSpeed(timeRate);
        audio.SetAudio(directoryName + chart.AudioPath);

        return new Playfield(main, skin, chart, audio, settings, timeRate);
    }

    public ResultScreen GetResultScreen(Main main, Chart chart, ScoreSystem scoreSystem)
    {
        PackedScene resultScreenScene = GD.Load<PackedScene>("res://Screens/ResultScreen.tscn");
        ResultScreen resultScreen = resultScreenScene.Instance<ResultScreen>();
        resultScreen.Init(main, chart, scoreSystem);
        return resultScreen;
    }
}