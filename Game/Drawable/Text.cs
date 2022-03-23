using Godot;

public class DrawableText : IDrawable
{
    private DynamicFont _font;
    private string _format;
    private string _text;
    private Align _horizontalAlign;
    private Align _verticalAlign;
    private Vector2 _textPosition;
    public Vector2 ExactPosition;

    public void SetText(float value)
    {
        _text = string.Format(_format, value);
        UpdatePosition();
    }

    public void SetText(string text)
    {
        _text = text;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        Vector2 fontSize = _font.GetStringSize(_text);
        fontSize.y = _font.GetAscent();
        _textPosition = Drawable.ApplyAlign(
            ExactPosition,
            fontSize,
            _horizontalAlign,
            _verticalAlign
        );
    }

    public DrawableText(string fontPath, int fontSize, string format, Align horizontal, Align vertical)
    {
        _font = new DynamicFont();
        _font.FontData = ResourceLoader.Load<DynamicFontData>(fontPath);
        _font.Size = fontSize;
        _format = format;
        _horizontalAlign = horizontal;
        _verticalAlign = vertical;

        ExactPosition = new Vector2();
    }

    public DrawableText(DynamicFont font, Vector2 position)
    {
        _font = font;
        ExactPosition = position;
        _horizontalAlign = Align.Left;
        _verticalAlign = Align.Top;
    }

    public void Draw(Node2D node)
    {
        node.DrawString(_font, _textPosition, _text);
    }
}