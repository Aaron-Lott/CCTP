using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Consumer Settings")]
public class ConsumerSettings : LivingEntitySettings
{
    [Tooltip("The age at which the creatures matures.")]
    public int maturityAge;

    [Tooltip("The size of the creature at birth.")]
    [Range(0.0f, 1.0f)]
    public float sizeAtBirth;

    [HideInInspector] public Vector3 ScaleAtBirth { get { return new Vector3(sizeAtBirth, sizeAtBirth, sizeAtBirth); } }
}
