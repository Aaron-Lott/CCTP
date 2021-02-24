using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Fish Settings")]
public class FishSettings : ConsumerSettings
{
    [Tooltip("Does this fish shoal/school?")]
    public bool doesSchool = false;

    [Tooltip("The range at which this creature can sense.")]
    [Range(1.0f, 10.0f)]
    public float schoolPerceptiveRange = 5f;
}
