using Godot;
using System;

public class SongSelect : Node2D
{
    private Main _main;
    private LineEdit _lineEdit;

    public void Init(Main main)
    {
        _main = main;
        _lineEdit = GetNode<LineEdit>("LineEdit");
    }

    public void StartChart()
    {
        _main.StartChart(_lineEdit.Text);
    }
}
