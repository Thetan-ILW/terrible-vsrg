using Godot;

public class DrawableText : IDrawable
{
    private DynamicFont _font;
    private Vector2 _position;
    public string Text { private get; set; }

    public DrawableText(string fontPath, int fontSize, Vector2 position)
    {
        _font = new DynamicFont();
        _font.FontData = ResourceLoader.Load<DynamicFontData>(fontPath);
        _font.Size = fontSize;

        _position = position;
    }

    public DrawableText(DynamicFont font, Vector2 position)
    {
        _font = font;
        _position = position;
    }

    public void Draw(Node2D node)
    {
        node.DrawString(_font, _position, Text);
    }
}