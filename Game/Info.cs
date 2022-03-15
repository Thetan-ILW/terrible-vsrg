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

    private JudgeDrawable _judgeDrawable;
    private ErrorBar _errorBar;

    public void Construct(Skin skin, ScoreSystem scoreSystem)
    {
        _skin = skin;

        _scoreSystem = scoreSystem;
        _base = (BaseScoreSystem)_scoreSystem.Container["Base"];
        _wife = (WifeScoreSystem)_scoreSystem.Container["Wife"];
        _judge = (JudgeScoreSystem)_scoreSystem.Container["Judge"];

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

        _judgeDrawable = new JudgeDrawable(
            skin.JudgeImage[0],
            skin.JudgeImage[1],
            skin.JudgeImage[2],
            skin.JudgeImage[3],
            skin.JudgeRect
        );

        _errorBar = new ErrorBar(
            skin.ErrorBarPosition,
            10
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
        _judgeDrawable.Update(_judge.LastJudge);
        _errorBar.Update(_base.LastDelta, _judge.LastJudge);

        Update();
    }

    public override void _Draw()
    {
        _accuracyText.Draw(this);
        _comboText.Draw(this);
        _judgeDrawable.Draw(this);
        _errorBar.Draw(this);
    }
}
