public struct Chart
{
    public string Artist { get; private set; }
    public string Title { get; private set; }
    public string Difficulty { get; private set; }
    public string AudioPath { get; private set; }
    public int InputMode { get; private set; }
    public Note[] Notes;

    public Chart(string artist, string title, string difficulty, string audioPath, int inputMode, Note[] notes)
    {
        Artist = artist;
        Title = title;
        Difficulty = difficulty;
        AudioPath = audioPath;
        InputMode = inputMode;
        Notes = notes;
    }
}

public interface IParser
{
    Chart GetChart(string chartPath);
}