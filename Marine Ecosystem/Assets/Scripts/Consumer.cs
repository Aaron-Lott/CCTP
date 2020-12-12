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

    protected ConsumerSettings consumerSettings;

    protected bool isMature = false;

    protected override void Init()
    {
        base.Init();

        if (Gender == Gender.Male) entityName = settings.GetRandomMaleName();
        else if (Gender == Gender.Female) entityName = settings.GetRandomFemaleName();
        else entityName = null;

        consumerSettings = (ConsumerSettings)settings;
        transform.localScale = consumerSettings.ScaleAtBirth;
        StartCoroutine(GrowRoutine());
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

        if (hunger >= 1f)
        {
            //Die(CauseOfDeath.Hunger);
        }

        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

        if (moveDirection.normalized != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection.normalized), 0.15f);
    }

    protected virtual void ChooseNextAction()
    {
        Act();
    }

    protected virtual void Act()
    {
        currentAction = CreatureAction.Socialising;

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

    protected override void Die(CauseOfDeath cause)
    {
        if(!dead)
        {
            StartCoroutine(DieRoutine());
        }

        base.Die(cause);
    }

    protected IEnumerator DieRoutine()
    {
        var anim = GetComponent<Animator>();

        if(anim)
        {
            anim.SetTrigger("die");
        }

        Vector3 waterSurface = new Vector3(transform.position.x, 0.5f, transform.position.z);

        float randNum = Random.Range(0.0f, 1.0f);

        while (transform.position.y < waterSurface.y)
        {

            transform.position = Vector3.MoveTowards(transform.position, waterSurface, Time.deltaTime * 0.5f);
            yield return null;
        }

        yield return new WaitForSeconds(5f);

        StartCoroutine(FadeOutRoutine());
    }

    protected IEnumerator GrowRoutine()
    {
        float diff = 1.0f - consumerSettings.sizeAtBirth;

        while (age < consumerSettings.maturityAge && !dead)
        {
            transform.localScale = consumerSettings.ScaleAtBirth + (Vector3.one * (age / consumerSettings.maturityAge) * diff);
            yield return null;
        }

        isMature = true;
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
