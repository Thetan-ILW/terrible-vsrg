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
                    node.DrawTextureRect(
                        _skin.Keys[i].Texture,
                        new Godot.Rect2(
                            _skin.Keys[i].Position.x,
                            _skin.Keys[i].Position.y,
                            _skin.Keys[i].Size.x,
                            _skin.Keys[i].Size.y
                        ),
                        false
                    );
                    break;

                case true:
                    node.DrawTextureRect(
                        _skin.PressedKeys[i].Texture,
                        new Godot.Rect2(
                            _skin.PressedKeys[i].Position.x,
                            _skin.PressedKeys[i].Position.y,
                            _skin.PressedKeys[i].Size.x,
                            _skin.PressedKeys[i].Size.y
                        ),
                        false
                    );
                    break;
            }
        }
    }
}