public class DrawableKeys : IDrawable
{
    private Skin _skin;
    private bool[] _keyState; 

    public DrawableKeys(Skin skin)
    {
        _skin = skin;
    }

    public void Update(bool[] keyState)
    {
        _keyState = keyState;
    }

    public void Draw(Godot.Node2D node)
    {
        for (int i = 0; i != _skin.InputMode; i++)
        {
            switch(_keyState[i])
            {
                case false:
                    node.DrawTexture(
                        _skin.KeyImage[i],
                        new Godot.Vector2(
                            _skin.Position.x + (i * _skin.NoteSize.x),
                            _skin.Position.y
                        )
                    );
                    break;

                case true:
                    node.DrawTexture(
                        _skin.KeyPressedImage[i],
                        new Godot.Vector2(
                            _skin.Position.x + (i * _skin.NoteSize.x),
                            _skin.Position.y
                        )
                    );
                    break;
            }
        }
    }
}