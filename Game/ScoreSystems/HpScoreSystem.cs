using Godot;
using System;

public class HpScoreSystem : IScoreSystem
{
    public float hp {get; private set;}
    public bool isDead {get; private set;}

    private float hpIncrease = 0.05f;
    private float hpDecrease = 0.1f;

    public HpScoreSystem(float hp, float hpIncrease, float hpDecrease)
    {
        this.hp = hp;
        this.isDead = false;
        this.hpIncrease = hpIncrease;
        this.hpDecrease = hpDecrease;
    }

    public void ProcessHit(float deltaTime)
    {
        if (isDead)
            return;

        if (hp < 1)
            hp += hpIncrease;
        
        if (hp > 1)
            hp = 1;
    }

    public void ProcessMiss()
    {
        if (isDead)
            return;

        hp -= hpDecrease;

        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
        }
    }
}
