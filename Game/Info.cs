using System;
using Godot;

public class Info : Node2D
{
    private Skin _skin;

    private ScoreSystem _scoreSystem;
    private BaseScoreSystem _base;
    private WifeScoreSystem _wife;

    private DrawableText _accuracyText;
    private DrawableText _comboText;

    public void Construct(Skin skin, ScoreSystem scoreSystem)
    {
        _skin = skin;

        _scoreSystem = scoreSystem;
        _base = (BaseScoreSystem)_scoreSystem.Container["Base"];
        _wife = (WifeScoreSystem)_scoreSystem.Container["Wife"];

        _accuracyText = new DrawableText(
            "res://Assets/Roboto-Light.ttf",
            48,
            new Vector2(5, 40)
        );

        _comboText = new DrawableText(
            "res://Assets/Roboto-Light.ttf",
            48,
            new Vector2(612, 300)
        );

        UpdateValues();
    }

    public void UpdateValues()
    {
        _accuracyText.Text = string.Format(
            _skin.AccuracyFormat,
            _wife.Accuracy
        );

        _comboText.Text = _base.Combo.ToString();

        Update();
    }

    public override void _Draw()
    {
        _accuracyText.Draw(this);
        _comboText.Draw(this);
    }
}
