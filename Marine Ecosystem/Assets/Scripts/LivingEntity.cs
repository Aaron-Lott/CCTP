using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public Species species;

    protected bool dead;

    protected int timeScale = 60;

    protected float age = 0;
    protected int ageInYears = 0;
    public int lifeSpan = 5;

    //External file data.
    JSONReader jsonReader;

    string speciesText;
    string knownAsText;
    string factText;
    string nameText;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        JSONReader jsonReader = FindObjectOfType<JSONReader>();

        if(jsonReader)
        {
            speciesText = jsonReader.GetLivingEntityData(species).species;
            knownAsText = jsonReader.GetLivingEntityData(species).knownAs;
            factText = jsonReader.GetLivingEntityData(species).fact;
            nameText = jsonReader.GetRandomName();
        }
    }

    protected virtual void Update()
    {
        age += Time.deltaTime;

        ageInYears = Mathf.FloorToInt(age / timeScale); // 1 year = 1 minute

        if (ageInYears >= lifeSpan)
        {
            Die(CauseOfDeath.Age);
        }

        DisplayInformationPanel();
    }

    protected virtual void Die(CauseOfDeath cause)
    {
        if (!dead)
        {
            dead = true;
            //Destroy(gameObject);
        }
    }

    public void DisplayInformationPanel()
    {
        if(CameraController.Instance.DetectEntity() == this)
        {
            UIManager.Instance.UpdateInformationPanel(speciesText,
                ageInYears >= 1 ? ageInYears + " years" : Mathf.FloorToInt(age / (timeScale / 12)) + " months", knownAsText, nameText, factText);

            SetShaderOutline(0.04f);
        }
        else
        {
            SetShaderOutline(0.0f);
        }
    }

    public void SetShaderOutline(float value)
    {
        var renderer = gameObject.GetComponentInChildren<Renderer>();

        if (renderer)
        {
            renderer.sharedMaterial.SetFloat("_FirstOutlineWidth", value);
        }
    }
}
