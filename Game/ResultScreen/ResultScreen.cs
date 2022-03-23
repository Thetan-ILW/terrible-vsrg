using Godot;
using System;
using System.Collections.Generic;

public class ResultScreen : Node2D
{
    private Main _main;

    private Dictionary<string, DrawableText> info;

    private ScoreSystem _scoreSystem;

    public ResultScreen() {}
    public ResultScreen(Main main, ScoreSystem scoreSystem)
    {
        _main = main;
        _scoreSystem = scoreSystem;
        var baseScoreSystem = (BaseScoreSystem)_scoreSystem.Container["Base"];
        var wife = (WifeScoreSystem)_scoreSystem.Container["Wife"];

        var font = new DynamicFont();
        font.FontData = ResourceLoader.Load<DynamicFontData>("res://Assets/Roboto-Light.ttf");
        font.Size = 48;

        info = new Dictionary<string, DrawableText>();
        
        info.Add("MaxCombo", new DrawableText(font, new Vector2(0,0)));
        info.Add("Accuracy", new DrawableText(font, new Vector2(0,40)));
        info["MaxCombo"].SetText("Max combo: " + Convert.ToString(baseScoreSystem.MaxCombo));
        info["Accuracy"].SetText("Accuracy: " + string.Format("{0:P2}", wife.Accuracy));
    }

    public override void _Draw()
    {
        foreach(var text in info)
        {
            text.Value.Draw(this);
        }
    }

    public override void _Input(InputEvent input)
    {
        if(Input.IsActionJustPressed("return_to_song_select"))
            _main.SetToSongSelect();
    }
}
