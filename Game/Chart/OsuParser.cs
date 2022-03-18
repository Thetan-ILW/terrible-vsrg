using System;
using System.IO;
using System.Collections.Generic;

public class OsuParser : IChartParser
{
    public Chart GetChart(string chartPath)
    {
        string[] fileLines = File.ReadAllLines(chartPath);

        var general = GetInfoSection(fileLines, "[General]");
        var metadata = GetInfoSection(fileLines, "[Metadata]");
        var difficulty = GetInfoSection(fileLines, "[Difficulty]");

        //TimingPoint[] _timing;
        Note[] notes = GetNotes(
            fileLines,
            Convert.ToInt32(difficulty["CircleSize"])
        );

        return new Chart(
            metadata["Artist"],
            metadata["Title"],
            metadata["Version"],
            general["AudioFilename"],
            Convert.ToInt32(difficulty["CircleSize"]),
            notes
        );
    }

    private Dictionary<string, string> GetInfoSection(string[] fileLines, string sectionName)
    {
        int startIndex = GetStartIndex(ref fileLines, sectionName);
        Dictionary<string, string> section = new Dictionary<string, string>();
        for (int i = startIndex; i != fileLines.GetLength(0); i++)
        {
            string line = fileLines[i];
            if (line == "")
                break;

            string[] keyValue = line.Split(':');

            if (keyValue.GetLength(0) == 1)
            {
                section.Add(keyValue[0].Trim(), "");
                continue;
            }

            section.Add(keyValue[0].Trim(), keyValue[1].Trim());
        }

        return section;
    }

    private TimingPoint[] GetTimingPoints(string[] fileLines)
    {
        int startIndex = GetStartIndex(ref fileLines, "[TimingPoints]");
        // No idea how timing points works!
        throw new NotImplementedException();
    }

    private Note[] GetNotes(string[] fileLines, int inputMode)
    {
        int startIndex = GetStartIndex(ref fileLines, "[HitObjects]");
        int noteCount = 0;
        for (int i = startIndex; i != fileLines.GetLength(0); i++)
        {
            string line = fileLines[i];
            if (line == "")
                break;

            noteCount++;
        }

        Note[] notes = new Note[noteCount];
        for (int i = startIndex; i != startIndex + noteCount; i++)
        {
            string line = fileLines[i];
            string[] noteParam = line.Split(',');

            noteParam[5] = noteParam[5].Substring(
                0, 
                noteParam[5].IndexOf(":")
            );

            switch (noteParam[3])
            {
                case "1":
                    notes[i - startIndex] = BuildShortNote(noteParam, inputMode);
                    break;
                case "5":
                    notes[i - startIndex] = BuildShortNote(noteParam, inputMode);
                    break;
                case "128":
                    notes[i - startIndex] = BuildLongNote(noteParam, inputMode);
                    break;
                default:
                    throw new Exception("Invalid note type");
            }
        }

        return notes;
    }

    private int GetColumn(float value, int inputMode)
    {
        return (int)Math.Floor((value / 512f) * inputMode);
    }

    private Note BuildShortNote(string[] noteParam, int inputMode)
    {
        return new Note(
            time: Convert.ToSingle(noteParam[2]), 
            column: GetColumn(
                Convert.ToSingle(noteParam[0]),
                inputMode
            ),
            type: NoteType.ShortNote,
            length: 0
        );
    }

    private Note BuildLongNote(string[] noteParam, int inputMode)
    {
        float length = Convert.ToSingle(noteParam[5]) - Convert.ToSingle(noteParam[2]);
        return new Note(
            time: Convert.ToSingle(noteParam[2]), 
            column: GetColumn(
                Convert.ToSingle(noteParam[0]),
                inputMode
            ),
            type: NoteType.LongNote,
            length: length
        );
    }

    private int GetStartIndex(ref string[] fileLines, string sectionName)
    {
        int startIndex = 1;
        foreach (string line in fileLines)
        {
            if (line == sectionName)
                break;

            startIndex++;
        }
        return startIndex;
    }
}