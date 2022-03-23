using Godot;

public interface IDrawable
{
    void Draw(Godot.Node2D Node2d);
}

public enum Align
{
    Left,
    Top,
    Middle,
    Bottom,
    Right
}

public class Drawable
{
    public static Vector2 ApplyAlign(Vector2 exactPosition, Vector2 drawableSize, Align horizontalAlign, Align verticalAlign)
    {
        Vector2 newPosition = new Vector2();
        switch (horizontalAlign)
        {
            case Align.Left:
                newPosition.x = exactPosition.x;
                break;
            case Align.Middle:
                newPosition.x = exactPosition.x - (drawableSize.x * 0.5f);
                break;
            case Align.Right:
                newPosition.x = exactPosition.x - drawableSize.x;
                break;
        }

        switch (verticalAlign)
        {
            case Align.Top:
                newPosition.y = exactPosition.y + drawableSize.y;
                break;
            case Align.Middle:
                newPosition.y = exactPosition.y + (drawableSize.y * 0.5f);
                break;
            case Align.Bottom:
                newPosition.y = exactPosition.y;
                break;
        }

        return newPosition;
    }
}