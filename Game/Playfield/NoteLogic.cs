public class NoteLogic
{
    private ScoreSystem _scoreSystem;

    public float CurrentTime = 0;
    public int NextExistingNote = 0;

    private float _lateMiss = 155;

    public NoteLogic(ScoreSystem scoreSystem, float lateMiss,float timeRate)
    {
        _scoreSystem = scoreSystem;
        _lateMiss = lateMiss * timeRate;
    }

    public void Process(ref Note[] notes)
    {
        for (int i = NextExistingNote; i != notes.GetLength(0) ; i++)
        {
            if (!notes[i].IsExist)
                continue;

            if (notes[i].Time + _lateMiss < CurrentTime)
            {
                _scoreSystem.ProcessMiss();
                notes[i].IsExist = false;
                if (i + 1 != notes.GetLength(0))
                    NextExistingNote = i + 1;
            }

            if (notes[i].IsExist == true)
                return;
        }
    }
}