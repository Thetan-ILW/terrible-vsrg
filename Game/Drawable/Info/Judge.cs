using Godot;
public class JudgeDrawable : IDrawable
{

    private Texture _maxTexture;
    private Texture _goodTexture;
    private Texture _badTexture;
    private Texture _missTexture;

    private Texture _currentTexture;
    private Vector2 _position;

    public JudgeDrawable(Texture maxTexture, Texture goodTexture, Texture badTexture, Texture missTexture, Vector2 position)
    {
        _maxTexture = maxTexture;
        _goodTexture = goodTexture;
        _badTexture = badTexture;
        _missTexture = missTexture;
        _position = position;
    }

    public void Update(JudgeScoreSystem.JudgeName judgeName)
    {
        if (judgeName == JudgeScoreSystem.JudgeName.Max)
            _currentTexture = _maxTexture;
        else if (judgeName == JudgeScoreSystem.JudgeName.Good)
            _currentTexture = _goodTexture;
        else if (judgeName == JudgeScoreSystem.JudgeName.Bad)
            _currentTexture = _badTexture;
        else if (judgeName == JudgeScoreSystem.JudgeName.Miss)
            _currentTexture = _missTexture;
    }

    public void Draw(Godot.Node2D node)
    {
        node.DrawTexture(_currentTexture, _position);
    }
}