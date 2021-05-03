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
        {
            gameData.AchievementCount++;
            gameData.AchievementsUnlocked[(int)type] = true;
        }
    }

    public bool AchievementIsUnlocked(AchievementTypes type)
    {
       return gameData.AchievementsUnlocked[(int)type];
    }

    public int GetAchievementCount()
    {
        return gameData.AchievementCount;
    }

    public void SetAchievementCount(int amount)
    {
        gameData.AchievementCount = amount;
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

    public void UnlockWhitetipReefShark()
    {
        if (gameData.WhitetipReefSharkUnlocked == false)
        {
            gameData.WhitetipReefSharkUnlocked = true;
        }
    }

    public bool WhitetipReefSharkIsUnlocked()
    {
        return gameData.WhitetipReefSharkUnlocked;
    }

    public void UnlockMoorishIdol()
    {
        if (gameData.MoorishIdolUnlocked == false)
        {
            gameData.MoorishIdolUnlocked = true;
        }
    }

    public bool MoorishIdolIsUnlocked()
    {
        return gameData.MoorishIdolUnlocked;
    }

    public void UnlockButterflyFish()
    {
        if (gameData.ButterflyFishUnlocked == false)
        {
            gameData.ButterflyFishUnlocked = true;
        }
    }

    public bool ButterflyFishIsUnlocked()
    {
        return gameData.ButterflyFishUnlocked;
    }
}
