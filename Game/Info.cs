using Godot;

public class Info : Node2D
{
    private Skin _skin;

    private ScoreSystem _scoreSystem;
    private BaseScoreSystem _base;
    private WifeScoreSystem _wife;

    private RichTextLabel _comboCount;
    private RichTextLabel _accuracy;

    public void Construct(Skin skin, ScoreSystem scoreSystem)
    {
        _skin = skin;

        _scoreSystem = scoreSystem;
        _base = (BaseScoreSystem)_scoreSystem.Container["Base"];
        _wife = (WifeScoreSystem)_scoreSystem.Container["Wife"];

        _comboCount = GetNode<RichTextLabel>("ComboCount");
        _accuracy = GetNode<RichTextLabel>("Accuracy");

        _comboCount.SetPosition(skin.ComboPosition);
        UpdateValues();
    }

    public void UpdateValues()
    {
        _comboCount.Text = _base.Combo.ToString();
        _accuracy.Text = string.Format(
            _skin.AccuracyFormat,
            _wife.Accuracy
        );
    }
}
