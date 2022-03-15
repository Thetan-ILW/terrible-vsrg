public class DrawablePressedKeys
{
    public void Draw(Godot.Node2D node, Skin skin, bool[] keyState)
    {
        for (int i = 0; i != skin.InputMode; i++)
        {
            if (keyState[i] != true)
                continue;
            
            node.DrawTexture(
                skin.KeyPressedImage[i],
                new Godot.Vector2(
                    skin.Position.x + (i * skin.NoteSize.x),
                    skin.Position.y
                )
            );
        }
    }
}