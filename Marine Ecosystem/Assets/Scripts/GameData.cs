using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool[] AchievementsUnlocked = new bool[9];

    public int AchievementCount = 0;

    [Range(-80, 0)]
    public float MasterVolume = 0f;

    public bool MusicIsOn = true;

    [Range(0, 2)]
    public int GraphicsQuality = 2;

    public bool InstructionsEnabled = true;
}

public enum AchievementTypes
{
    STARFISH_DETECTIVE = 0,
    ODD_OFFSPRING = 1,
    THIS_MILESTONE_IS_GARBAGE = 2,
    FLUORESCING_CORAL = 4,
    CRUMBLING_AWAY = 3,
    CORAL_BLEACHING = 7,
    ITS_NOT_TOO_LATE = 8,

}