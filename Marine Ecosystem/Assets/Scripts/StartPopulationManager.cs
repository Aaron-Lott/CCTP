using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPopulationManager : MonoBehaviour
{
    public StartPopulationSlider[] fishSliders;
    public StartPopulationSlider[] coralSliders;

    public void SetStartPopulationValues()
    {
        StartPopulationValues.Clownfish = fishSliders[0].count;
        StartPopulationValues.Angelfish = fishSliders[1].count;
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
}
