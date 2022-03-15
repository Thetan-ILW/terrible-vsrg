using Godot;
public class JudgeDrawable : IDrawable
{

    private Texture _maxTexture;
    private Texture _goodTexture;
    private Texture _badTexture;
    private Texture _missTexture;

    private Texture _currentTexture;
    private Rect2 _rect;

    public JudgeDrawable(Texture maxTexture, Texture goodTexture, Texture badTexture, Texture missTexture, Rect2 rect)
    {
        _maxTexture = maxTexture;
        _goodTexture = goodTexture;
        _badTexture = badTexture;
        _missTexture = missTexture;
        _rect = rect;
    }

    public void Update(JudgeScoreSystem.JudgeName judgeName)
    {
        switch (judgeName)
        {
            case JudgeScoreSystem.JudgeName.Max:
                _currentTexture = _maxTexture;
                break;
            case JudgeScoreSystem.JudgeName.Good:
                _currentTexture = _goodTexture;
                break;
            case JudgeScoreSystem.JudgeName.Bad:
                _currentTexture = _badTexture;
                break;
            case JudgeScoreSystem.JudgeName.Miss:
                _currentTexture = _missTexture;
                break;
        }
    }

    public void Draw(Godot.Node2D node)
    {
        node.DrawTextureRect(_currentTexture, _rect, false);
    }
}