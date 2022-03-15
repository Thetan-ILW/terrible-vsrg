public class DrawableNotes : Drawable
{
    public void Draw(Godot.Node2D node, Note[] notes, Skin skin, int nextExistingNote, float currentTime, float scrollSpeed)
    {
        for (int i = nextExistingNote; i != notes.GetLength(0) ; i++)
        {
            Note note = notes[i];
            // Считаем для каждой ноты из массива нот положение на экране
            float noteYPosition = (-note.Time + currentTime + (skin.Position.y / scrollSpeed)) * scrollSpeed;

            // Прерываем рисовку нот если хоть одна нота уже за экраном
            // Потому что после неё уже все ноты будут за границей видимости
            if (noteYPosition < -skin.NoteSize.y)
                break; 

            if (note.IsExist != true)
                continue;
                     
            node.DrawTexture(
                skin.NoteImage[note.Column],
                new Godot.Vector2(
                    skin.Position.x + (note.Column * skin.NoteSize.x),
                    noteYPosition)
            );
        }
    }
}