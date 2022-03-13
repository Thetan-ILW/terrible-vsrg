using Godot;
using System;

public class ChartGenerator
{
    public Note[] DoMagic()
    {
        Random rnd = new Random();

        int noteCount = 100;
        float bpm = 89;
        float timeMs = (60000 / bpm) / 4;
        Note[] note = new Note[noteCount];

        int time = 0;

        for (int i = 0; i != noteCount; i++)
        {
            note[i] = new Note(time * timeMs, rnd.Next(0, 4), false, 1);
            i++;
            do {
                note[i] = new Note(time * timeMs, rnd.Next(0, 4), false, 1);
            } while (note[i].column == note[i - 1].column);
            time++;
        }
        return note;
    }
}
