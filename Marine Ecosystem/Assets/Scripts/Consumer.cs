using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumer : LivingEntity
{
    CreatureAction currentAction;

    protected float perceptiveRange = 5f;

    protected float moveSpeed = 2f;
    protected float fastMoveSpeed = 6f;

    [HideInInspector]
    public Vector3 moveDirection;

    protected float timeBetweenActionChoices = 1;
    protected float lastActionChooseTime;

    protected float hunger = 0;
    protected float timeUntilDeathByHunger = 100;

    protected float eatDuration = 10;

    protected float criticalPercent = 0.7f;

    protected LivingEntity foodTarget;

    protected override void Init()
    {
        base.Init();
    }

    protected override void Update()
    {

        base.Update();

        float timeSinceLastActionChoice = Time.time - lastActionChooseTime;

        if (timeSinceLastActionChoice > timeBetweenActionChoices)
        {
            ChooseNextAction();
        }

        hunger += Time.deltaTime * 1 / timeUntilDeathByHunger;

        if (hunger >= 1)
        {
            Die(CauseOfDeath.Hunger);
        }

        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

        if (moveDirection.normalized != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection.normalized), 0.15f);
    }

    protected virtual void ChooseNextAction()
    {
        /*lastActionChooseTime = Time.time;

        bool currentlyEating = currentAction == CreatureAction.Eating && foodTarget && hunger > 0;
        if (hunger >= criticalPercent || currentlyEating) {
            SearchForFood();
        }*/

        //currentAction = CreatureAction.EscapingPredator;

        Act();
    }

    protected virtual void Act()
    {
        switch(currentAction)
        {
            case CreatureAction.GoingToFood:
                break;

            case CreatureAction.Socialising:
                Socialising();
                break;

            case CreatureAction.EscapingPredator:
                EscapeFromPredator();
                break;

        }
    }

    protected virtual void SearchForFood()
    {
        LivingEntity foodSource = null;
        if (foodSource)
        {
            currentAction = CreatureAction.GoingToFood;
            foodTarget = foodSource;
        }
        else
        {
            currentAction = CreatureAction.EscapingPredator;
        }
    }

    void HandleInteractions()
    {
        if (currentAction == CreatureAction.Eating)
        {
            if (foodTarget && hunger > 0)
            {
                float eatAmount = Mathf.Min(hunger, Time.deltaTime * 1 / eatDuration);
                eatAmount = ((Producer)foodTarget).Consume(eatAmount);
                hunger -= eatAmount;
            }
        }
    }

    protected virtual void EscapeFromPredator()
    {

    }

    protected virtual void Socialising()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
