public class OsuScoreSystem : IScoreSystem
{
    private struct TimingWindows
    {
        public float Marvelous;
        public float Perfect;
        public float Great;
        public float Good;
        public float Bad;

        public TimingWindows(int od)
        {
            float odDecrease = 3 * od;

            Marvelous = 16.5f;
            Perfect = 64.5f - odDecrease;
            Great = 97.5f - odDecrease;
            Good = 127.5f - odDecrease;
            Bad = 151.5f - odDecrease;
        }
    }

    private struct JudgeCount
    {
        public int Marvelous;
        public int Perfect;
        public int Great;
        public int Good;
        public int Bad;
        public int Total;
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
        GetJudge(System.Math.Abs(deltaTime));
        _jc.Total++;

        float maxPoints = 300 * (_jc.Marvelous + _jc.Perfect);
        float greatPoins = 200 * _jc.Great;
        float goodPoints = 100 * _jc.Good;
        float badPoints = 50 * _jc.Bad;
        float totalPoints = 300 * _jc.Total;

        Accuracy = (badPoints + goodPoints + greatPoins + maxPoints) / totalPoints;
    }

    public void ProcessMiss()
    {
        _jc.Total++;
    }

    private void GetJudge(float deltaTime)
    {
        if (deltaTime <= _tw.Marvelous) {_jc.Marvelous++; return;}
        if (deltaTime <= _tw.Perfect) {_jc.Perfect++; return;}
        if (deltaTime <= _tw.Great) {_jc.Great++; return;}
        if (deltaTime <= _tw.Good) {_jc.Good++; return;}
        if (deltaTime <= _tw.Bad) {_jc.Bad++; return;}
        // else miss++
    }
}