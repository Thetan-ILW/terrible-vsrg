using Godot;

public class Info : Node2D
{
    private Skin _skin;

    private ScoreSystem _scoreSystem;
    private BaseScoreSystem _base;
    private WifeScoreSystem _wife;
    private JudgeScoreSystem _judge;

    private DrawableText _accuracyText;
    private DrawableText _comboText;

    private JudgeDrawable _judgeImage;
    private ErrorBar _errorBar;

    public Info() {}
    public Info(Skin skin, ScoreSystem scoreSystem)
    {
        _skin = skin;

        _scoreSystem = scoreSystem;
        _base = (BaseScoreSystem)_scoreSystem.Container["Base"];
        _wife = (WifeScoreSystem)_scoreSystem.Container["Wife"];
        _judge = (JudgeScoreSystem)_scoreSystem.Container["Judge"];

        _accuracyText = skin.AccuracyText;
        _comboText = skin.ComboText;
        _judgeImage = skin.JudgeImage;
        _errorBar = skin.ErrorBar;

        UpdateValues();
    }

    public void UpdateValues()
    {
        _accuracyText.SetText(_wife.Accuracy);
        _comboText.SetText(_base.Combo);
        _judgeImage.Update(_judge.LastJudge);
        _errorBar.Update(_base.LastDelta, _judge.LastJudge);

        Update();
    }

    public override void _Draw()
    {
        _accuracyText.Draw(this);
        _comboText.Draw(this);
        _judgeImage.Draw(this);
        _errorBar.Draw(this);
    }
}
