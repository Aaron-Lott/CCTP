using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementMenuManager : MonoBehaviour
{
    [SerializeField] private Image[] achievementIcons;

    private void Start()
    {
        for(int i = 0; i < achievementIcons.Length; i++)
        {
            if(achievementIcons[i] != null)
            {
                if (GameDataController.Instance.AchievementIsUnlocked((AchievementTypes)i))
                {
                    Unlocked(i);
                }
                else
                {
                    Locked(i);
                }
            }
        }
    }

    private void Unlocked(int i)
    {
        Text[] achievementTexts = achievementIcons[i].GetComponentsInChildren<Text>();

        foreach (Text text in achievementTexts)
        {
            text.enabled = true;
        }

        achievementIcons[i].color = new Color(1, 1, 1, 1);
    }

    private void Locked(int i)
    {
        Text[] achievementTexts = achievementIcons[i].GetComponentsInChildren<Text>();

        foreach (Text text in achievementTexts)
        {
            text.enabled = false;
        }

        achievementIcons[i].color = new Color(1, 1, 1, 0.15f);
    }
}

