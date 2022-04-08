using Godot;

public class Background : Sprite
{
    private ImageTexture _imageTexture;

    private float _brightness = 1f;
    public float Brightness 
    {
        get
        {
            return _brightness;
        }
        set
        {
            _brightness = value;
            SetBrightness(value);
        }
    }

    public override void _Ready()
    {
        _imageTexture = new ImageTexture();
    }

    public void SetBackground(string imageName)
    {
        _imageTexture.Load(imageName);
        Texture = _imageTexture;
        Resize(OS.WindowSize);
    }

    public void SetBrightness(float value)
    {
        Modulate = new Color(1f * value, 1f * value, 1f * value, 1f);
    }

    public void Resize(Vector2 windowSize)
    {
        Vector2 textureSize = Texture.GetSize();
        Scale = new Vector2(
            windowSize.y / textureSize.y,
            windowSize.y / textureSize.y
        );
    }
}
