using System;

public class WifeScoreSystem : IScoreSystem
{
    private float _timingScale = 1;
    private float _points = 0f;
    private float _maxPoints = 0f;
    private float _miss = 0;

    public float Accuracy { get; private set; }

    public WifeScoreSystem(float timingScale)
    {
        _timingScale = timingScale;
    }

    public void ProcessHit(float deltaTime)
    {
        _maxPoints += 2;
        ProcessPoints(deltaTime);
        Update();
    }

    public void ProcessMiss()
    {
        _maxPoints += 2;
        _miss++;
        Update();
    }

    public void Update()
    {
        Accuracy = (_points - (_miss * 8)) / _maxPoints;
    }

    private void ProcessPoints(float deltaTime)
    {
        double avedeviation = 95f * _timingScale;
        double y = 1 - (Math.Pow(2f, (-1f * deltaTime * deltaTime / (avedeviation * avedeviation))));
        y = Math.Pow(y, 2);
        _points += (2 - -8) * (1 - Convert.ToSingle(y)) + -8;
    }
}