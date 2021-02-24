using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool[] AchievementsUnlocked = new bool[9];

    [Range(-80, 0)]
    public float MasterVolume = 0f;

    public bool MusicIsOn = true;

    [Range(0, 2)]
    public int GraphicsQuality = 2;
}

public enum AchievementTypes
{
    FLUORESCING_CORAL = 0,
    THIS_ACHIEVEMENT_IS_GARBAGE = 1,
    ITS_NOT_TOO_LATE = 8
}