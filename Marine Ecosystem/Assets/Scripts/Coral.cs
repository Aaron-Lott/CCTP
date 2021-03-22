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

    private Color orignalColor;
    public Color fluoresceColor;

    private float fluoresceTemp;
    private float deadTemp;

    public ParticleSystem eatingEffect;

    private bool hasFluoresced = false;
    private bool hasDied = false;

    protected override void Init()
    {
        base.Init();
        coralSettings = (CoralSettings)settings;

        fluoresceTemp = Environment.Instance.SeaTemperature + Random.Range(2.0f, 2.5f);
        deadTemp = fluoresceTemp + 1f;
        orignalColor = _renderer.material.GetColor("_Color");
    }

    public override float Consume(float amount, Consumer consumer)
    {
        if(eatingEffect)
        {
            eatingEffect.transform.position = consumer.transform.position;

            if (consumer.Hunger > 0.1f)
            {
                if (!eatingEffect.isPlaying)
                    eatingEffect.Play();
            }
            else
            {
                eatingEffect.Stop();
            }

            if (!consumer)
            {
                eatingEffect.Stop();
            }
        }

            return amountRemaining;
    }

    protected override void Update()
    {
        base.Update();

        if (!DoesFluoresce)
            return;

        if(Environment.Instance.SeaTemperature >= fluoresceTemp)
        {
            if(!hasFluoresced)
            {
                if (Species == Species.StaghornCoral)
                {
                    StarfishManager.Instance.SpawnStarfish(AchievementTypes.FLUORESCING_CORAL, transform.position + new Vector3(0, 4, 0));
                }

                StartCoroutine(LerpColor(orignalColor, fluoresceColor));
                hasFluoresced = true;
            }

            if(Environment.Instance.SeaTemperature >= deadTemp && !hasDied)
            {
                if(!hasDied)
                {
                    if (Species == Species.StaghornCoral)
                    {
                        StarfishManager.Instance.SpawnStarfish(AchievementTypes.CORAL_BLEACHING, transform.position + new Vector3(0, 4, 0));
                    }

                    StartCoroutine(LerpColor(fluoresceColor, DeadColor));
                    hasDied = true;
                }
            }
        }
    }

    protected IEnumerator LerpColor(Color start, Color target)
    {
        float duration = Environment.Instance.TimeScale;
        float increment = Time.deltaTime / (duration / 2f);
        float progress = 0f;

        while (progress < 1f)
        {
            _renderer.material.SetColor("_Color", Color.Lerp(start, target, progress));
            progress += increment;

            health -= increment / 2f;

            yield return null;
        }
        
    }
}
