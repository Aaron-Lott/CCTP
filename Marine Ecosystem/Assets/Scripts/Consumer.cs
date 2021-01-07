using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumer : LivingEntity
{
    public CreatureAction CurrentAction { get; private set; }

    protected string entityName = null;

    protected float moveSpeed = 2f;
    protected float fastMoveSpeed = 6f;


    protected Vector3 moveDirection;
    protected Vector3 moveTarget;
    protected Vector3 randomTarget;

    protected float timeBetweenActionChoices = 1;
    protected float lastActionChooseTime;

    protected float hunger = 0;

    protected float eatDuration = 8;

    protected float criticalHungerPercent = 0.7f;

    protected bool currentActionIsImportant = false;

    public float CriticalHungerPercent { get { return criticalHungerPercent;  } }

    protected LivingEntity foodTarget;

    protected Consumer mateTarget;

    protected ConsumerSettings consumerSettings;

    protected bool isMature = false;

    public float Hunger { get { return hunger; } }

    public int MaxLifeSpan { get { return consumerSettings.lifeSpan.y; } }
    public int MinLifeSpan { get { return consumerSettings.lifeSpan.x; } }

    protected Species[] Diet {get {return consumerSettings.diet; } }

    protected float PerceptiveRange { get { return consumerSettings.perceptiveRange;  } }

    public Gender Gender { get { return consumerSettings.gender; } }

    public string RandomName { get { return entityName; } }

    protected override void Init()
    {
        base.Init();

        consumerSettings = (ConsumerSettings)settings;

        lifeSpan = consumerSettings.GetLifeSpan();

        if (Gender == Gender.Male) entityName = consumerSettings.GetRandomMaleName();
        else if (Gender == Gender.Female) entityName = consumerSettings.GetRandomFemaleName();
        else entityName = null;

        transform.localScale = consumerSettings.ScaleAtBirth;
        StartCoroutine(GrowRoutine());

        SimulatingAge = true;

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
            transform.position += move * moveSpeed * Time.deltaTime;

            if (move != Vector3.zero)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(move), 0.15f);
        }
    }

    protected virtual void ChooseNextAction()
    {
        if (hunger >= criticalHungerPercent)
        {
                foodTarget = GetClosestFoodSource(PerceptiveRange);        
        }
        else if (isMature && !mateTarget)
        {
            mateTarget = GetClosestMate(PerceptiveRange);
        }

        if (hunger < 1 - criticalHungerPercent)
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
                if (entity.Species == this.Species && entity != this)
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
            //CurrentAction = CreatureAction.Mating;
        }

        return closestTarget;
    }

    protected void  Eat(LivingEntity food)
    {
        if (food)
        {
            var targetPos = food.transform.position + new Vector3(0, 1 , 0);

            if (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                moveDirection = (targetPos - transform.position);
            }
            else
            {
                moveDirection = Vector3.zero;

                if (food && hunger > 0)
                {
                    float eatAmount = Mathf.Min(hunger, Time.deltaTime * 1 / eatDuration);
                    eatAmount = ((Producer)food).Consume(eatAmount);
                    hunger -= eatAmount * 1.5f;
                }
            }
        }
    }

    protected void Mate(Consumer mate)
    {
        if (mate)
        {
            var targetPos = mate.transform.position;

            if (Vector3.Distance(transform.position, targetPos) > 0.5f)
            {
                moveDirection = (targetPos - transform.position);
            }
            else
            {
                moveDirection = Vector3.zero;
                currentActionIsImportant = true;
            }
        }
    }

    protected override void Die(CauseOfDeath cause)
    {
        if(!dead)
        {
            RemoveFromLists();
            StartCoroutine(DieRoutine());
        }

        base.Die(cause);
    }

    protected IEnumerator DieRoutine()
    {
        var anim = GetComponentInChildren<Animator>();

        if(anim)
        {
            anim.SetTrigger("die");
        }

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
        health = (1.0f - (age / MaxLifeSpan) - (hunger / eatDuration));
    }

    protected override void UpdateInfoPanel()
    {
        UIManager.Instance.UpdateInfoPanelConsumer(this);
    }

    protected IEnumerator GetMoveTarget()
    {
        while(!dead)
        {
            randomTarget = BoidManager.Instance.GetRandomTarget();

            float randTime = Random.Range(4, 12);
            yield return new WaitForSeconds(randTime);
            yield return null;
        }
    }

}
