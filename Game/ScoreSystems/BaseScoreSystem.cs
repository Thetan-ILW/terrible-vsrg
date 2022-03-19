public class BaseScoreSystem : IScoreSystem
{
    public int Hits { get; private set; }
    public int Combo { get; private set; }
    public int MaxCombo { get; private set; }
    public int Miss { get; private set; }
    public float LastDelta { get; private set; }

    public void ProcessHit(float deltaTime)
    {
        Hits++;
        Combo++;
        MaxCombo = System.Math.Max(MaxCombo, Combo);
        LastDelta = deltaTime;
    }

    public void ProcessMiss()
    {
        Combo = 0;
        Miss++;
        LastDelta = 155f;
    }
}