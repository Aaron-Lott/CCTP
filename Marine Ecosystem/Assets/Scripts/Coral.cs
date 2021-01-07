using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coral : Producer
{
    protected CoralSettings coralSettings;

    public bool DoesFluoresce { get { return coralSettings.doesFluoresce; } }

    public Color FluoresceBlue { get { return coralSettings.FluoresceBlue(); } }
    public Color FluoresceYellow { get { return coralSettings.FluoresceYellow(); } }
    public Color FluorescePurple { get { return coralSettings.FluorescePurple(); } }

    public Color DeadColor { get { return coralSettings.DeadColor(); } }

    private float fluoresceTemp;

    protected override void Init()
    {
        base.Init();
        coralSettings = (CoralSettings)settings;

        fluoresceTemp = Environment.Instance.SeaTemperature + Random.Range(0.5f, 1f);

        if(DoesFluoresce)
        StartCoroutine(FluoresceRoutine(FluoresceBlue));
    }

    protected override void Update()
    {
        base.Update();
    }

    protected IEnumerator FluoresceRoutine(Color fluorescentColor)
    {
        string colorString = "_Color";
        var startColor = _renderer.material.GetColor(colorString);
        bool fluoresced = false;

        while (!dead)
        {
            float t = 0.05f * Mathf.PingPong(Time.time, 1);

            var currentColor = _renderer.material.GetColor(colorString);

            if (Environment.Instance.SeaTemperature < fluoresceTemp)
            {
                //return to original color.
                if (currentColor != startColor)
                {
                    _renderer.material.SetColor(colorString, Color.Lerp(currentColor, startColor, t));
                }
                else if(fluoresced)
                {
                    fluoresced = false;
                }
            }
            else
            {
                //turn to a fluorescent color.
                if (currentColor != fluorescentColor && !fluoresced)
                {
                    _renderer.material.SetColor(colorString, Color.Lerp(currentColor, fluorescentColor, t));
                }
                else 
                {
                    fluoresced = true;
                }

                if(fluoresced)
                {
                    //turn to dead color.
                    _renderer.material.SetColor(colorString, Color.Lerp(currentColor, DeadColor, t));
                }

            }

            yield return null;
        }
    }
}
