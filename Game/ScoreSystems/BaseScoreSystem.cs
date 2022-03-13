public class BaseScoreSystem : IScoreSystem
{
    public int Hits {get; private set;}
    public int Combo {get; private set;}
    public int Miss {get; private set;}

    public void ProcessHit(float deltaTime)
    {
        Hits++;
        Combo++;
    }

    public void ProcessMiss()
    {
        Combo = 0;
        Miss++;
    }
}