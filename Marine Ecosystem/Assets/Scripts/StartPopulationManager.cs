using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPopulationManager : MonoBehaviour
{
    public StartPopulationSlider[] fishSliders;
    public StartPopulationSlider[] coralSliders;

    public GameDataController gameDataController;

    public Image[] lockImages;
    public Image[] lockedSpeciesImages;

    private void Start()
    {
        if(gameDataController.GetAchievementCount() >= 3)
        {
            SetUIUnlocked(0);
            gameDataController.UnlockMoorishIdol();
        }

        if(gameDataController.GetAchievementCount() >= 6)
        {
            SetUIUnlocked(1);
            gameDataController.UnlockButterflyFish();
        }

        if(gameDataController.GetAchievementCount() >= 9)
        {
            SetUIUnlocked(2);
            gameDataController.UnlockWhitetipReefShark();
        }
    }

    public void SetStartPopulationValues()
    {
        StartPopulationValues.Clownfish = fishSliders[0].count;
        StartPopulationValues.Butterflyfish = fishSliders[1].count;
        StartPopulationValues.YellowTang = fishSliders[2].count;
        StartPopulationValues.Parrotfish = fishSliders[3].count;
        StartPopulationValues.BlacktipReefShark = fishSliders[4].count;
        StartPopulationValues.MoorishIdol = fishSliders[5].count;
        StartPopulationValues.Damselfish = fishSliders[6].count;
        StartPopulationValues.WhitetipReefShark = fishSliders[7].count;

        StartPopulationValues.StaghornCoralColony = coralSliders[0].count;
        StartPopulationValues.TableCoralColony = coralSliders[1].count;
        StartPopulationValues.YellowSpongeTubeColony = coralSliders[2].count;
    }

    public void SetUIUnlocked(int index)
    {
        lockImages[index].gameObject.SetActive(false);
        lockedSpeciesImages[index].color = Color.white;
    }
}
