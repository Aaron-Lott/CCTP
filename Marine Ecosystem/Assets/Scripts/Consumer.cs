using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumer : LivingEntity
{
    public CreatureAction CurrentAction { get; private set; }

    protected string entityName = null;


    protected Vector3 moveDirection;
    protected Vector3 moveTarget;
    public Vector3 randomTarget;

    protected float MoveSpeed { get; set; }

    protected float BaseMoveSpeed { get { return consumerSettings.moveSpeed; } }
    protected float FastMoveSpeed { get { return consumerSettings.moveSpeed * 2; } }

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
    protected bool MateFound { get; set; }

    protected Consumer Offspring { get { return consumerSettings.offspringPrefab;  } }

    public float Hunger { get { return hunger; } }

    public int EatDuration { get { return consumerSettings.eatDuration;  } }

    public int MaxLifeSpan { get { return consumerSettings.lifeSpan.y; } }
    public int MinLifeSpan { get { return consumerSettings.lifeSpan.x; } }

    protected Species[] Diet {get { return consumerSettings.diet; } }

    protected float PerceptiveRange { get { return consumerSettings.perceptiveRange;  } }

    protected int CriticalPopulation { get { return consumerSettings.criticalPopulation; } }

    protected GameObject MatingEffect { get { return consumerSettings.matingEffect;  } }

    public Gender Gender { get; set; }

    public string RandomName { get { return entityName; } }

    protected override void Init()
    {
        base.Init();

        consumerSettings = (ConsumerSettings)settings;

        lifeSpan = consumerSettings.GetRandomLifeSpan();

        MoveSpeed = Random.Range(BaseMoveSpeed * 0.8f, BaseMoveSpeed * 1.2f);

        if (Gender == Gender.Male) entityName = consumerSettings.GetRandomMaleName();
        else if (Gender == Gender.Female) entityName = consumerSettings.GetRandomFemaleName();
        else entityName = null;

        transform.localScale = consumerSettings.ScaleAtBirth;
        StartCoroutine(GrowRoutine());

        SetInitialAgeAndHunger();

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

            Vector3 move = moveDirection.normalized + moveTarget.normalized;
            transform.position += move * MoveSpeed * Time.deltaTime;

            if (move != Vector3.zero)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(move), 0.15f);
        }
    }

    protected virtual void ChooseNextAction()
    {
        bool currentlyEating = CurrentAction == CreatureAction.Foraging && foodTarget && hunger > 0;
        bool currentlyMating = CurrentAction == CreatureAction.Mating && mateTarget && mateTarget.mateTarget == this && MateFound;

        if(hunger >= criticalHungerPercent)
        {
                foodTarget = GetClosestFoodSource(PerceptiveRange);        
        }
        else if (isMature && !MateFound && !hasMated && Environment.Instance.EntityPopulations[Species] < CriticalPopulation && !currentlyEating)
        {
            if (Gender == Gender.Male)
            {
                mateTarget = GetClosestMate(PerceptiveRange);
            }

            if (mateTarget)
            {
                CurrentAction = CreatureAction.Mating;
                MateFound = true;
            }
            else
            {
                CurrentAction = CreatureAction.Exploring;
            }

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
                Eat(foodTarget, new Vector3(0, 1, 0));
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

    #region Forgaging Behaviour

    protected virtual LivingEntity GetClosestFoodSource(float range)
    {
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, range);

        float distanceToClosestTarget = Mathf.Infinity;
        LivingEntity closestTarget = null;

        foreach (var collider in collidersInRange)
        {
            LivingEntity entity = collider.GetComponent<LivingEntity>();

            if (entity)
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

        if (closestTarget)
        {
            CurrentAction = CreatureAction.Foraging;
        }
        else
        {
            CurrentAction = CreatureAction.Exploring;
        }

        return closestTarget;
    }

    protected void Eat(LivingEntity food, Vector3 offset = default)
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
                        ((Producer)food).Consume(eatAmount, this);
                    }
                    else if (food is Consumer)
                    {
                        food.transform.parent = transform;
                        ((Consumer)food).Die(CauseOfDeath.Eaten);
                    }

                }
            }
        }
    }

    #endregion

    #region Mating Behaviour

    protected virtual Consumer GetClosestMate(float range)
    {
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, range);

        float distanceToClosestTarget = Mathf.Infinity;
        Consumer closestTarget = null;

        foreach (var collider in collidersInRange)
        {
            Consumer entity = collider.GetComponent<Consumer>();

            if (entity)
            {
                if (entity.Species == this.Species && entity.Gender != this.Gender)
                {
                    float distanceToTarget = (entity.transform.position - transform.position).sqrMagnitude;

                    if (distanceToTarget < distanceToClosestTarget)
                    {
                        distanceToClosestTarget = distanceToTarget;
                        closestTarget = entity;
                        closestTarget.mateTarget = this;
                    }
                }
            }
        }

        return closestTarget;
    }

    protected void Mate(Consumer mate)
    {
        if (!hasMated)
        {
            var targetPos = mate.transform.position;

            if (Vector3.Distance(transform.position, targetPos) > 0.2f)
            {
                moveDirection = (targetPos - transform.position);
            }
            else
            {
                moveDirection = Vector3.zero;

                if (Gender == Gender.Female)
                {
                    StartCoroutine(MateRoutine(8f, mate));
                }
            }
        }
    }

    protected IEnumerator MateRoutine(float duration, Consumer mate)
    {
        hasMated = true;

        GameObject matingEffectGo = null;

        Vector3 spawnPos = (mateTarget.transform.position + transform.position) / 2;


        if (MatingEffect)
        {
            matingEffectGo = Instantiate(MatingEffect, spawnPos, Quaternion.Euler(-90, 0, 0));
        }

        yield return new WaitForSeconds(duration);

        for(int i = 0; i < consumerSettings.GetRandomOffspringCount(); i++)
        {
            Instantiate(Offspring, spawnPos, Quaternion.identity);
        }

        var matingPS = MatingEffect.GetComponent<ParticleSystem>();

        matingPS.Stop();

        if (matingPS.isStopped)
        {
            Destroy(matingEffectGo);
        }

        CurrentAction = CreatureAction.Exploring;
        mate.CurrentAction = CreatureAction.Exploring;

        yield return new WaitForSeconds(5f);

        MateFound = false;
        mate.MateFound = false;

        hasMated = false;
        mate.hasMated = false;
    }

    #endregion

    #region Exploring Behaviour

    protected virtual void Exploring()
    {
        if (Vector3.Distance(transform.position, randomTarget) > 0.1f)
        {
            moveTarget = randomTarget - transform.position;
        }
        else
        {
            moveTarget = Vector3.zero;
        }
    }

    protected virtual IEnumerator GetMoveTarget()
    {
        while (!dead)
        {
            randomTarget = Environment.Instance.GetRandomTarget();

            float randTime = Random.Range(4, 12);
            yield return new WaitForSeconds(randTime);
        }
    }

    #endregion

    #region Death Behaviour

    protected override void Die(CauseOfDeath cause)
    {
        if (!dead)
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

    #endregion

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

    protected void SetInitialAgeAndHunger()
    {
        age = Random.Range(0, consumerSettings.maturityAge);
        hunger = Random.Range(0, criticalHungerPercent / 2);
    }

    protected virtual void EscapeFromPredator()
    {

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
}
