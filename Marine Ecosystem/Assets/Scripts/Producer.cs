using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producer : LivingEntity
{
    float amountRemaining = 1;
    const float consumeSpeed = 8;

    protected override void Init()
    {
        base.Init();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool SimulateAge()
    {
        return false;
    }

    public float Consume(float amount)
    {
        float amountConsumed = Mathf.Max(0, Mathf.Min(amountRemaining, amount));
        amountRemaining -= amount * consumeSpeed;

        //transform.localScale = Vector3.one * amountRemaining;

        if (amountRemaining <= 0)
        {
            //Die(CauseOfDeath.Eaten);
        }

        return amountConsumed;
    }

    public float AmountRemaining
    {
        get
        {
            return amountRemaining;
        }
    }
}
