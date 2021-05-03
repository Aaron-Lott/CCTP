using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalPollution : Pollution
{
    private Color startColor;
    private Color pollutedColor;

    protected override void Init()
    {
        base.Init();
        startColor = RenderSettings.fogColor;
    }

    protected override void Update()
    {
        base.Update();
        RenderSettings.fogColor = Color.Lerp(startColor, Colors.ChemicalPollution, Environment.Instance.ChemicalPollutionLevels);
    }


}
