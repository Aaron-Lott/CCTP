using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Living Entity Settings")]
public class LivingEntitySettings : ScriptableObject
{
    [Tooltip("The species of this creature.")]
    public Species species;

    [Tooltip("The gender of this creature.")]
    public Gender gender;

    [Tooltip("The scientific name given to this creature.")]
    public string scientificSpecies;

    [Tooltip("A fact about this creature.")]
    public string fact;

    [Tooltip("The minimum (X) and maximum (Y) lifespan of this creature.")]
    public Vector2Int lifeSpan;

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
        return (Gender)Random.Range(1, 2);
    }

}
