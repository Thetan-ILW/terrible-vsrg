public class DrawableNotes : IDrawable
{
    private Note[] _notes;
    private Skin _skin;

    private int _nextExistingNote;
    private float _currentTime;
    private float _scrollSpeed; 

    public DrawableNotes(ref Note[] notes, Skin skin)
    {
        _notes = notes;
        _skin = skin;
    }

    public void Update(int nextExistingNote, float currentTime, float scrollSpeed)
    {
        _nextExistingNote = nextExistingNote;
        _currentTime = currentTime;
        _scrollSpeed = scrollSpeed;
    }

    public void Draw(Godot.Node2D node)
    {
        for (int i = _nextExistingNote; i != _notes.GetLength(0) ; i++)
        {
            Note note = _notes[i];
            // Считаем для каждой ноты из массива нот положение на экране
            float noteYPosition = (-note.Time + _currentTime + (_skin.HitPosition / (_scrollSpeed / _skin.Ratio.y))) * (_scrollSpeed / _skin.Ratio.y);
            //float noteYPosition = (-note.Time + _currentTime) * (_scrollSpeed / _skin.Ratio);

            // Прерываем рисовку нот если хоть одна нота уже за экраном
            // Потому что после неё уже все ноты будут за границей видимости
            if (noteYPosition < -_skin.Notes[0].Size.y)
                break; 

            if (note.IsExist != true)
                continue;
                     
            node.DrawTextureRect(
                _skin.Notes[note.Column].Texture,
                new Godot.Rect2(
                    _skin.Notes[note.Column].Position.x,
                    noteYPosition,
                    _skin.Notes[note.Column].Size.x,
                    _skin.Notes[note.Column].Size.y
                ),
                false
            );
        }
    }
}