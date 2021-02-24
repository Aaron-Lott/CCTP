using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    public static Achievements Instance;


    public AchievementSettings FluorescingCorals;
    public AchievementSettings ThisAchievementIsGarbage;
    public AchievementSettings ItsNotTooLate;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

}
