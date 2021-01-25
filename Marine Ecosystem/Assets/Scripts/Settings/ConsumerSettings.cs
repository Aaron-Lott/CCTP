using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Consumer Settings")]
public class ConsumerSettings : LivingEntitySettings
{
    [Tooltip("The gender of this creature.")]
    public Gender gender;

    [Tooltip("The offspring of this creature.")]
    public Consumer offspringPrefab;

    [Tooltip("The speed at which this creature moves.")]
    [Range(0.0f, 10.0f)]
    public float moveSpeed = 2;

    [Tooltip("The minimum (X) and maximum (Y) lifespan of this creature.")]
    public Vector2Int lifeSpan;

    [Tooltip("The age at which the creatures matures.")]
    public int maturityAge;

    [Tooltip("The size of the creature at birth.")]
    [Range(0.0f, 1.0f)]
    public float sizeAtBirth;

    [HideInInspector] public Vector3 ScaleAtBirth { get { return new Vector3(sizeAtBirth, sizeAtBirth, sizeAtBirth); } }

    [Tooltip("The diet of this creature.")]
    public Species[] diet;

    [Tooltip("The range at which this creature can sense.")]
    [Range(1.0f, 20.0f)]
    public float perceptiveRange = 5;

    [Tooltip("How long it takes for the creature to eat it's food.")]
    [Range(1, 20)]
    public int eatDuration = 10;

    [Tooltip("The effect instaniated when mating.")]
    public GameObject matingEffect;

    [Tooltip("A text file used to generate a random female name for this creature.")]
    [SerializeField] private TextAsset femaleNameFile = null;

    [Tooltip("A text file used to generate a random male name for this creature.")]
    [SerializeField] private TextAsset maleNameFile = null;

    public string GetRandomFemaleName()
    {
        string[] contents = femaleNameFile.text.Split("\n"[0]);
        int random = Random.Range(1, contents.Length);

        return contents[random];
    }

    public string GetRandomMaleName()
    {
        string[] contents = maleNameFile.text.Split("\n"[0]);
        int random = Random.Range(1, contents.Length);

        return contents[random];
    }

    public float GetLifeSpan()
    {
        return Random.Range((float)lifeSpan.x, (float)lifeSpan.y);
    }

    public Gender GetRandomGender()
    {
        return (Gender)Random.Range(0, 2);
    }

}
