public class HpScoreSystem : IScoreSystem
{
    public float Hp {get; private set;}
    public bool IsDead {get; private set;}

    private float _hpIncrease = 0.05f;
    private float _hpDecrease = 0.1f;

    public HpScoreSystem(float hp, float hpIncrease, float hpDecrease)
    {
        Hp = Hp;
        IsDead = false;
        _hpIncrease = hpIncrease;
        _hpDecrease = hpDecrease;
    }

    public void ProcessHit(float deltaTime)
    {
        if (IsDead)
            return;

        if (Hp < 1)
            Hp += _hpIncrease;
        
        if (Hp > 1)
            Hp = 1;
    }

    public void ProcessMiss()
    {
        if (IsDead)
            return;

        Hp -= _hpDecrease;

        if (Hp <= 0)
        {
            Hp = 0;
            IsDead = true;
        }
    }
}
