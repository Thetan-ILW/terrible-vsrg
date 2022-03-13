using Godot;
using System;

public class NoteLogic
{
    private ScoreSystem _scoreSystem;

    public float CurrentTime = 0;
    public int NextExistingNote = 0;

    private float _lateMiss = 155;

    public NoteLogic(ScoreSystem scoreSystem)
    {
        _scoreSystem = scoreSystem;
    }

    public void Process(ref Note[] note)
    {
        for (int i = NextExistingNote; i != note.GetLength(0) ; i++)
        {
            if (!note[i].isExist)
                continue;

            if (note[i].time + _lateMiss < CurrentTime)
            {
                _scoreSystem.ProcessMiss();
                note[i].isExist = false;
                if (i + 1 != note.GetLength(0))
                    NextExistingNote = i + 1;
            }

            if (note[i].isExist == true)
                return;
        }
    }
}