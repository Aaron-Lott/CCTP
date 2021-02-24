using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Settings/Achievement Settings")]
public class AchievementSettings : ScriptableObject
{
    public string achievementTitle;

    public string achievementText;

    public Sprite medalImage;

    public Sprite achievementArt;
}
