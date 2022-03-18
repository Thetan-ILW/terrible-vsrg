using Godot;
using System;

public class SongSelect : Node2D
{
    private Main _main;
    private LineEdit _chartPath;

    public void Init(Main main)
    {
        _main = main;
        _chartPath = GetNode<LineEdit>("ChartPath");
    }

    public void StartChart()
    {
        _main.StartChart(_chartPath.Text);
    }
}
