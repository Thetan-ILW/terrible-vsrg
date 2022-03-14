using System;

public class ChartGenerator
{
    public Note[] DoMagic()
    {
        Random rnd = new Random();

        int noteCount = 100;
        float bpm = 89;
        float timeMs = (60000 / bpm) / 4;
        Note[] notes = new Note[noteCount];

        int time = 0;

        for (int i = 0; i != noteCount; i++)
        {
            notes[i] = new Note(time * timeMs, rnd.Next(0, 4), false, 1);
            i++;
            do {
                notes[i] = new Note(time * timeMs, rnd.Next(0, 4), false, 1);
            } while (notes[i].column == notes[i - 1].column);
            time++;
        }
        return notes;
    }
}
