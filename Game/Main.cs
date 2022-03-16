using Godot;
using System;

public class Main : Node2D
{
    private OsuParser _osuParser;
    private Chart _chart;
    private Audio _audio;

    private SongSelect _songSelect;
    private Playfield _playfield;

    public override void _Ready()
    {
        PackedScene songSelect = GD.Load<PackedScene>("res://Nodes/SongSelect.tscn");
        _songSelect = songSelect.Instance<SongSelect>();
        _songSelect.Init(this);
        AddChild(_songSelect);
    }

    public void StartChart(string path)
    {
        string directoryName = path.Substring(
            0, 
            path.LastIndexOf("/") + 1
        );

        string chartName = path.Substring(directoryName.Length);

        _osuParser = new OsuParser();
        _chart = _osuParser.GetChart(directoryName + chartName);

        _audio = new Audio();
        _audio.SetAudio(
            directoryName + _chart.AudioPath
        );

        _songSelect.Visible = false;
        _playfield = new Playfield(_chart, _audio);
        AddChild(_playfield);
    }
}
