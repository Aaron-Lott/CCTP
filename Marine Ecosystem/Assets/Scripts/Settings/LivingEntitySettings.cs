using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Living Entity Settings")]
public class LivingEntitySettings : ScriptableObject
{
    [Tooltip("The species of this creature.")]
    public Species species;

    [Tooltip("The scientific name given to this creature.")]
    public string scientificSpecies;

    [Tooltip("A fact about this creature.")]
    public string fact;
}
