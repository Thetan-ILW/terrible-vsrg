using Godot;
using System;
using System.Collections.Generic;

public class ResultScreen : Control
{
    private Main _main;
    private ScoreSystem _scoreSystem;

    public void Init(Main main,Chart chart, ScoreSystem scoreSystem)
    {
        _main = main;
        _scoreSystem = scoreSystem;

        var baseScoreSystem = (BaseScoreSystem)_scoreSystem.Container["Base"];
        var judge = (JudgeScoreSystem)_scoreSystem.Container["Judge"];
        var wife = (WifeScoreSystem)_scoreSystem.Container["Wife"];

        _main.GetTree().Root.Connect("size_changed", this, nameof(SetWindowSize));
        SetWindowSize();

        SetHeader(chart);
        SetPlayInfo(wife, judge);
    }

    private void SetHeader(Chart chart)
    {
        var title = GetNode<Label>("Header/ChartName");
        title.Text = $"{chart.Artist} - {chart.Title} [{chart.Difficulty}]";
    }

    private void SetPlayInfo(WifeScoreSystem wife, JudgeScoreSystem judge)
    {
        var accuracy = GetNode<Label>("Info/Player/Accuracy");
        var max = GetNode<Label>("Info/Player/MaxCount");
        var good = GetNode<Label>("Info/Player/GoodCount");
        var bad = GetNode<Label>("Info/Player/BadCount");
        var miss = GetNode<Label>("Info/Player/MissCount");

        accuracy.Text += string.Format("{0:P2}", wife.Accuracy);
        max.Text += judge.Count.Max.ToString();
        good.Text += judge.Count.Good.ToString();
        bad.Text += judge.Count.Bad.ToString();
        miss.Text += judge.Count.Miss.ToString();
    }

    public void SetWindowSize() => RectSize = OS.WindowSize;

    public override void _Input(InputEvent input)
    {
        if(Input.IsActionJustPressed("return_to_song_select"))
            _main.SetToSongSelect();
    }
}
