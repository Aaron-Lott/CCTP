using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumer : LivingEntity
{
    public CreatureAction CurrentAction { get; private set; }

    protected string entityName = null;


    protected Vector3 moveDirection;
    protected Vector3 moveTarget;
    protected Vector3 randomTarget;

    protected float timeBetweenActionChoices = 1;
    protected float lastActionChooseTime;

    protected float hunger = 0;

    protected float criticalHungerPercent = 0.7f;

    public float CriticalHungerPercent { get { return criticalHungerPercent;  } }

    protected LivingEntity foodTarget;

    protected Consumer mateTarget;

    protected ConsumerSettings consumerSettings;

    protected bool isMature = false;
    protected bool hasMated = false;

    protected Consumer Offspring { get { return consumerSettings.offspringPrefab;  } } 

    protected float MoveSpeed { get { return consumerSettings.moveSpeed; } }
    protected float FastMoveSpeed { get { return consumerSettings.moveSpeed * 2; } }


    public float Hunger { get { return hunger; } }

    public int EatDuration { get { return consumerSettings.eatDuration;  } }

    public int MaxLifeSpan { get { return consumerSettings.lifeSpan.y; } }
    public int MinLifeSpan { get { return consumerSettings.lifeSpan.x; } }

    protected Species[] Diet {get {return consumerSettings.diet; } }

    protected float PerceptiveRange { get { return consumerSettings.perceptiveRange;  } }

    protected GameObject MatingEffect { get { return consumerSettings.matingEffect;  } }

    public Gender Gender { get; set; }

    public string RandomName { get { return entityName; } }

    protected override void Init()
    {
        base.Init();

        consumerSettings = (ConsumerSettings)settings;

        Gender = consumerSettings.GetRandomGender();

        lifeSpan = consumerSettings.GetLifeSpan();

        if (Gender == Gender.Male) entityName = consumerSettings.GetRandomMaleName();
        else if (Gender == Gender.Female) entityName = consumerSettings.GetRandomFemaleName();
        else entityName = null;

        transform.localScale = consumerSettings.ScaleAtBirth;
        StartCoroutine(GrowRoutine());

        SimulatingAge = true;

        CurrentAction = CreatureAction.Exploring;

        StartCoroutine(GetMoveTarget());
    }

    protected override void Update()
    {
        base.Update();

        float timeSinceLastActionChoice = Time.time - lastActionChooseTime;

        if (timeSinceLastActionChoice > timeBetweenActionChoices && !dead)
        {
            ChooseNextAction();
        }

        if(Environment.Instance != null)

        hunger += Time.deltaTime * 1 / Environment.Instance.TimeScale;

        if (hunger >= 1f)
        {
            Die(CauseOfDeath.Hunger);
        }

        if(!dead)
        {
            if(CurrentAction != CreatureAction.Exploring)
            {
                moveTarget = Vector3.zero;
            }

            Vector3 move = moveDirection.normalized  + moveTarget.normalized;
            transform.position += move * MoveSpeed * Time.deltaTime;

            if (move != Vector3.zero)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(move), 0.15f);
        }
    }

    protected virtual void ChooseNextAction()
    {
        bool currentlyEating = CurrentAction == CreatureAction.Foraging && foodTarget && hunger > 0;
        bool currentlyMating = CurrentAction == CreatureAction.Mating && mateTarget;

        if (hunger >= criticalHungerPercent)
        {
                foodTarget = GetClosestFoodSource(PerceptiveRange);        
        }
        else if (isMature && !hasMated)
        {
            //mateTarget = GetClosestMate(PerceptiveRange);
        }
        else if(!currentlyEating && !currentlyMating)
        {
            CurrentAction = CreatureAction.Exploring;
        }

        Act();
    }

    protected virtual void Act()
    {

        switch (CurrentAction)
        {
            case CreatureAction.Resting:
                break;

            case CreatureAction.Foraging:
                Eat(foodTarget);
                break;

            case CreatureAction.Exploring:
                Exploring();
                break;

            case CreatureAction.Mating:
                Mate(mateTarget);
                break;

            case CreatureAction.EscapingPredator:
                EscapeFromPredator();
                break;

        }
    }

    protected virtual LivingEntity GetClosestFoodSource(float range)
    {
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, range);

        float distanceToClosestTarget = Mathf.Infinity;
        LivingEntity closestTarget = null;

        foreach (var collider in collidersInRange)
        {
            LivingEntity entity = collider.GetComponent<LivingEntity>();

            if(entity)
            {
                foreach (Species food in Diet)
                {
                    if (entity.Species == food)
                    {
                        float distanceToTarget = (entity.transform.position - transform.position).sqrMagnitude;

                        if (distanceToTarget < distanceToClosestTarget)
                        {
                            distanceToClosestTarget = distanceToTarget;
                            closestTarget = entity;
                        }
                    }
                }
            }        
        }

        if(closestTarget)
        {
            CurrentAction = CreatureAction.Foraging; 
        }
        else
        {
            CurrentAction = CreatureAction.Exploring;
        }

        return closestTarget;
    }

    protected virtual Consumer GetClosestMate(float range)
    {
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, range);

        float distanceToClosestTarget = Mathf.Infinity;
        Consumer closestTarget = null;

        foreach (var collider in collidersInRange)
        {
            Consumer entity = collider.GetComponent<Consumer>();

            if(entity)
            {
                if (entity.Species == this.Species && entity.Gender == Gender.Female && Gender == Gender.Male)
                {
                    float distanceToTarget = (entity.transform.position - transform.position).sqrMagnitude;

                    if (distanceToTarget < distanceToClosestTarget)
                    {
                        distanceToClosestTarget = distanceToTarget;
                        closestTarget = entity;
                    }
                }
            }
        }

        if (closestTarget)
        {
            CurrentAction = CreatureAction.Mating;
        }
        else
        {
            CurrentAction = CreatureAction.Exploring;
        }

        return closestTarget;
    }

    protected void  Eat(LivingEntity food, Vector3 offset = default)
    {
        if (food)
        {
            var targetPos = food.transform.position + offset;

            if (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                moveDirection = (targetPos - transform.position);
            }
            else
            {
                moveDirection = Vector3.zero;

                if (hunger > 0)
                {
                    float eatAmount = Time.deltaTime * 1 / EatDuration;
                    hunger -= eatAmount;

                    if (food is Producer)
                    {
                        ((Producer)food).Consume(eatAmount * (2.0f - CriticalHungerPercent));
                    }
                    else if(food is Consumer)
                    {
                        food.transform.parent = transform;
                        ((Consumer)food).Die(CauseOfDeath.Eaten);
                    }

                }
            }
        }
    }

    protected void Mate(Consumer mate)
    {
        if (mate && !hasMated)
        {
            var targetPos = mate.transform.position;

            if (Vector3.Distance(transform.position, targetPos) > 0.5f)
            {
                moveDirection = (targetPos - transform.position);
            }
            else
            {
                moveDirection = Vector3.zero;

                if(Gender == Gender.Female)
                {
                    StartCoroutine(MateRoutine(10f));
                }
            }
        }
    }

    protected IEnumerator MateRoutine(float duration)
    {
        hasMated = true;

        Vector3 spawnPos = (mateTarget.transform.position + transform.position) / 2;

        if(MatingEffect)
        {
            Instantiate(MatingEffect, spawnPos, Quaternion.Euler(-90, 0, 0));
        }

        yield return new WaitForSeconds(duration);

        Instantiate(Offspring, spawnPos, Quaternion.identity);
    }


    protected override void Die(CauseOfDeath cause)
    {
        if(!dead)
        {
            RemoveFromLists();

            if (anim)
            {
                anim.SetTrigger("die");
            }

            if (cause == CauseOfDeath.Eaten)
            {
                StartCoroutine(FadeOutRoutine());
            }
            else
            {
                StartCoroutine(FloatToWaterSurface());
            }
        }

        base.Die(cause);
    }

    protected IEnumerator FloatToWaterSurface()
    {
        Vector3 waterSurface = new Vector3(transform.position.x, 0.5f, transform.position.z);

        while (transform.position.y < waterSurface.y)
        {

            transform.position = Vector3.MoveTowards(transform.position, waterSurface, Time.deltaTime * 0.5f);
            yield return null;
        }

        yield return new WaitForSeconds(5f);

        StartCoroutine(FadeOutRoutine());
    }

    protected virtual void RemoveFromLists() { }

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

    protected virtual void Exploring()
    {
        if(Vector3.Distance(transform.position, randomTarget) > 0.1f)
        {
            moveTarget = randomTarget - transform.position;
        }
        else
        {
            moveTarget = Vector3.zero;
        }
    }

    public override void SimulateHealth()
    {
        health = (1.0f - (age / MaxLifeSpan) - (hunger / 4));
    }

    protected override void UpdateInfoPanel()
    {
        if(UIManager.Instance != null)
        UIManager.Instance.UpdateInfoPanelConsumer(this);
    }

    protected IEnumerator GetMoveTarget()
    {
        while(!dead)
        {
            randomTarget = Environment.Instance.GetRandomTarget();

            float randTime = Random.Range(4, 12);
            yield return new WaitForSeconds(randTime);
        }
    }

}
