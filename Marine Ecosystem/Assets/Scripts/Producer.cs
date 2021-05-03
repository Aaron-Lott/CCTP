using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producer : LivingEntity
{
    protected float amountRemaining = 1;

    protected Vector3 initalScale;

    protected override void Init()
    {
        base.Init();
        initalScale = transform.localScale;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void UpdateInfoPanel()
    {
        UIManager.Instance.UpdateInfoPanelProducer(this);
    }

    public virtual float Consume(float amount, Consumer consumer)
    {
        amountRemaining -= amount;

        transform.localScale = initalScale * Mathf.Min(amountRemaining, 1.0f);
        health = Mathf.Min(amountRemaining, 1.0f);

        if (amountRemaining <= 0.08f)
        {
            Die(CauseOfDeath.Eaten);
            Destroy(gameObject);
        }

        return amountRemaining;
    }

    protected override void Die(CauseOfDeath cause)
    {
        base.Die(cause);
        Destroy(gameObject);
    }

    public float AmountRemaining
    {
        get
        {
            return amountRemaining;
        }
    }
}
