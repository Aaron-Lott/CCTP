using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public Species Species { get { return settings.species; } }

    public Gender Gender { get { return settings.gender; } }

    protected bool dead;

    protected int timeScale = 60; // 1 year = 1 minute
    protected float elapsedTime = 0;

    protected float age = 0;

    protected float lifeSpan;

    public Vector3 orbitCamViewOffset = Vector3.zero;

    [Range(0.0f, 1.0f)]
    protected float health = 1.0f;

    protected string entityName = null;

    
    [SerializeField] protected LivingEntitySettings settings = null;

    public bool Dead { get { return dead; } }

    public float Age { get { return age; } }

    public int MaxLifeSpan { get { return settings.lifeSpan.y; } }
    public int MinLifeSpan { get { return settings.lifeSpan.x; } }

    public float Health {get { return health;  } }

    public string ScientificName { get { return settings.scientificSpecies; } }

    public string Fact { get { return settings.fact; } }

    public string RandomName {get { return entityName; } }

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        lifeSpan = settings.GetLifeSpan();
    }

    protected virtual void Update()
    {
        SimulateAge();

        health = (1.0f - age / MaxLifeSpan);

        if(DisplayInformationPanel())
        {
            UIManager.Instance.UpdateInformationPanel(this);

            SetShaderOutline(0.04f);
        }
        else
        {
            SetShaderOutline(0.0f);
        }
    }

    public virtual bool SimulateAge()
    {
        elapsedTime += Time.deltaTime;

        age = elapsedTime / timeScale;

        if (age >= lifeSpan)
        {
            Die(CauseOfDeath.Age);
        }

        return true;
    }

    protected virtual void Die(CauseOfDeath cause)
    {
        if (!dead)
        {
            dead = true;
        }
    }

    public bool DisplayInformationPanel()
    {
        if(CameraController.Instance.DetectEntity() == this)
        {
            return true;
        }

        return false;
    }

    public void SetShaderOutline(float value)
    {
        var renderer = gameObject.GetComponentInChildren<Renderer>();

        if (renderer)
        {
            renderer.sharedMaterial.SetFloat("_FirstOutlineWidth", value);
        }
    }

    protected IEnumerator FadeOutRoutine()
    {
        Renderer rend = GetComponentInChildren<Renderer>();

        if(rend)
        {
            rend.material.shader = Shader.Find("Transparent/Diffuse");

            while(rend.material.color.a > 0.0f)
            {
                float alpha = rend.material.color.a;
                Color newColor = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, Mathf.Lerp(alpha, 0.0f, Time.deltaTime * 0.5f));
                rend.material.color = newColor;
                yield return null;
            }
        }

        Destroy(gameObject);
    }
}
