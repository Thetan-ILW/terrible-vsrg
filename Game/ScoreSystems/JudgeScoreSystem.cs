public class JudgeScoreSystem : IScoreSystem
{
    public enum JudgeName
    {
        Max,
        Good,
        Bad,
        Miss,
        None
    }

    public struct TimingWindows
    {
        public float Max;
        public float Good;
        public float Bad;

        public TimingWindows(float max, float good, float bad)
        {
            Max = max;
            Good = good;
            Bad = bad;
        }
    }

    public struct JudgeCount
    {
        public int Max;
        public int Good;
        public int Bad;
        public int Miss;
    }

    private TimingWindows _timingWindows;
    public JudgeCount Count;
    public JudgeName LastJudge = JudgeName.None;

    public JudgeScoreSystem(float maxWindow, float goodWindow, float badWindow)
    {
        _timingWindows = new TimingWindows(
            maxWindow,
            goodWindow,
            badWindow
        );

        Count = new JudgeCount();
    }

    public void ProcessHit(float deltaTime)
    {
        deltaTime = System.Math.Abs(deltaTime);
        if (deltaTime <= _timingWindows.Max) 
        {
            Count.Max++; 
            LastJudge = JudgeName.Max;
            return;
        }
        else if (deltaTime <= _timingWindows.Good)
        {
            Count.Good++; 
            LastJudge = JudgeName.Good;
            return;
        }
        if (deltaTime <= _timingWindows.Bad) 
        {
            Count.Bad++; 
            LastJudge = JudgeName.Bad;
            return;
        }
    }

    public void ProcessMiss()
    {
        Count.Miss++;
        LastJudge = JudgeName.Miss;
    }
}