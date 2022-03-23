using Godot;

public class JudgeDrawable : IDrawable
{
    private Texture _maxTexture;
    private Texture _goodTexture;
    private Texture _badTexture;
    private Texture _missTexture;
    private Texture _currentTexture;

    private float _scale;
    private Align _horizontalAlign;
    private Align _verticalAlign;
    private Rect2 _rect;
    public Vector2 ExactPosition;

    public JudgeDrawable(Texture maxTexture, Texture goodTexture, Texture badTexture, Texture missTexture, float scale, Align horizontal, Align vertical)
    {
        _maxTexture = maxTexture;
        _goodTexture = goodTexture;
        _badTexture = badTexture;
        _missTexture = missTexture;
        _currentTexture = new ImageTexture();
        _scale = scale;
        _horizontalAlign = horizontal;
        _verticalAlign = vertical;
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
        UpdateRect();
    }

    public void UpdateRect()
    {
        Vector2 textureSize = _currentTexture.GetSize();
        textureSize.x *= _scale;
        textureSize.y *= _scale;
        
        Vector2 position = Drawable.ApplyAlign(
            ExactPosition,
            textureSize,
            _horizontalAlign,
            _verticalAlign
        );

        _rect = new Rect2(position, textureSize.x, textureSize.y);
    }

    public void Draw(Godot.Node2D node)
    {
        node.DrawTextureRect(_currentTexture, _rect, false);
    }
}