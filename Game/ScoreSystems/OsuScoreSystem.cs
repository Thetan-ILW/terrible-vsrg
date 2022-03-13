using Godot;
using System;

public class OsuScoreSystem : IScoreSystem
{
    private struct TimingWindows
    {
        public float marvelous;
        public float perfect;
        public float great;
        public float good;
        public float bad;

        public TimingWindows(int od)
        {
            float odDecrease = 3 * od;

            marvelous = 16.5f;
            perfect = 64.5f - odDecrease;
            great = 97.5f - odDecrease;
            good = 127.5f - odDecrease;
            bad = 151.5f - odDecrease;
        }
    }

    private struct JudgeCount
    {
        public int marvelous;
        public int perfect;
        public int great;
        public int good;
        public int bad;
        public int total;
    }

    TimingWindows _tw;
    JudgeCount _jc;

    public float Accuracy {get; private set;}
    private int _od = 8;

    public OsuScoreSystem(int od)
    {
        _od = od;
        _tw = new TimingWindows();
        _jc = new JudgeCount();
    }

    public void ProcessHit(float deltaTime)
    {
        GetJudge(Math.Abs(deltaTime));
        _jc.total++;

        float maxPoints = 300 * (_jc.marvelous + _jc.perfect);
        float greatPoins = 200 * _jc.great;
        float goodPoints = 100 * _jc.good;
        float badPoints = 50 * _jc.bad;
        float totalPoints = 300 * _jc.total;

        Accuracy = (badPoints + goodPoints + greatPoins + maxPoints) / totalPoints;
    }

    public void ProcessMiss()
    {
        _jc.total++;
    }

    private void GetJudge(float deltaTime)
    {
        if (deltaTime <= _tw.marvelous) {_jc.marvelous++; return;}
        if (deltaTime <= _tw.perfect) {_jc.perfect++; return;}
        if (deltaTime <= _tw.great) {_jc.great++; return;}
        if (deltaTime <= _tw.good) {_jc.good++; return;}
        if (deltaTime <= _tw.bad) {_jc.bad++; return;}
    }
}