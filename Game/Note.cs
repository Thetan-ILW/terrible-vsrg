public enum NoteType
{
    ShortNote,
    LongNote,
    Mine
}

public struct Note 
{
    public int Column { get; private set; }
    public float Time { get; private set; }
    public float Length { get; private set; }
    public NoteType Type { get; private set; }
    public bool IsExist { get; set; }

    public Note(float time, int column, NoteType type, float length)
    {
        Column = column;
        Time = time;
        Length = length;
        Type = type;
        IsExist = true;
    }
}
