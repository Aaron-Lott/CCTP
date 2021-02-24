using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameDataController : MonoBehaviour
{
    public GameData gameData;

    public static GameDataController Instance;

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

    public void UnlockAchievement(AchievementTypes type)
    {
        if(gameData.AchievementsUnlocked[(int)type] == false)
        gameData.AchievementsUnlocked[(int)type] = true;
    }

    public bool GetAchievementStatus(AchievementTypes type)
    {
       return gameData.AchievementsUnlocked[(int)type];
    }

    public float GetMasterVolume()
    {
        return gameData.MasterVolume;
    }

    public void SetMasterVolume(float volume)
    {
        gameData.MasterVolume = volume;
    }

    public int GetGraphicsQuality()
    {
        return gameData.GraphicsQuality;
    }

    public void SetGraphicsQuality(int quality)
    {
        gameData.GraphicsQuality = quality;
    }

    public bool GetMusicIsOn()
    {
        return gameData.MusicIsOn;
    }

    public void SetMusicIsOn(bool isOn)
    {
        gameData.MusicIsOn = isOn;
    }
}
