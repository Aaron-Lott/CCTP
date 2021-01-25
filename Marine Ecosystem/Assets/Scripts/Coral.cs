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

    Color orignalColor;
    private float fluoresceTemp;

    private enum Colors {ORIGINAL, FLUORESCED, DEAD}
    Colors colors = Colors.ORIGINAL;

    protected override void Init()
    {
        base.Init();
        coralSettings = (CoralSettings)settings;

        fluoresceTemp = Environment.Instance.SeaTemperature + Random.Range(0.5f, 1f);
        orignalColor = _renderer.material.GetColor("_Color");

        //if(DoesFluoresce)
        StartCoroutine(FluoresceRoutine(FluoresceBlue));
    }

    protected override void Update()
    {
        base.Update();
    }

    protected IEnumerator UpdateColor(Color fluorescentColor)
    {
        var currentColor = _renderer.material.GetColor("_Color");
        float currentTemp = Environment.Instance.seaTemperature;

        bool routineCalled = false;

        while(!dead)
        {
            if(!routineCalled)
            {
                if (currentTemp > fluoresceTemp)
                {
                    LerpColor(orignalColor, fluorescentColor);
                    routineCalled = true;
                }

                if(currentColor == fluorescentColor && currentTemp < 0)
                {

                }
            }
                yield return null;
        }
    }

    protected IEnumerator LerpColor(Color start, Color target)
    {
        float duration = 10f;
        float increment = Time.deltaTime / (duration / 2f);
        float progress = 0f;

        while (progress < 1f)
        {
            _renderer.material.SetColor("_Color", Color.Lerp(start, target, progress));
            progress += increment;
            yield return null;
        }
        
    }

    /*protected IEnumerator FluoresceRoutine(Color fluorescentColor)
    {
        var orignalColor = _renderer.material.GetColor("_Color");

        Colors color = Colors.ORIGINAL;

        while (!dead)
        {
            var currentColor = _renderer.material.GetColor("_Color");
            float currentTemp = Environment.Instance.seaTemperature;

            if(currentTemp >= fluoresceTemp && color == Colors.ORIGINAL)
            {
                LerpColor(orignalColor, fluorescentColor);
            }


            yield return null;
        }
    }*/


    protected IEnumerator FluoresceRoutine(Color fluorescentColor)
    {
        string colorString = "_Color";
        var startColor = _renderer.material.GetColor(colorString);
        bool fluoresced = false;

        while (!dead)
        {
            float t = Time.deltaTime * 0.5f;

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
