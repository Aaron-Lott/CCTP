using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public Species Species { get { return settings.species; } }

    protected bool dead;

    protected float age = 0;

    protected float lifeSpan;

    public Vector3 orbitCamViewOffset = Vector3.zero;

    [Range(0.0f, 1.0f)]
    protected float health = 1.0f;

    
    [SerializeField] protected LivingEntitySettings settings = null;

    public bool Dead { get { return dead; } }

    public float Age { get { return age; } }

    public bool SimulatingAge { get; protected set; } = false;

    public float Health {get { return health;  } }

    public string ScientificName { get { return settings.scientificSpecies; } }

    public string Fact { get { return settings.fact; } }

    protected Renderer _renderer;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _renderer = GetComponentInChildren<Renderer>();
    }

    protected virtual void Update()
    {
        if(SimulatingAge)
        SimulateAge();

        SimulateHealth();

        if(DisplayInformationPanel())
        {
            UpdateInfoPanel();
            SetShaderOutline(0.04f);
        }
        else
        {
            SetShaderOutline(0.0f);
        }
    }

    protected virtual void UpdateInfoPanel()
    {
        UIManager.Instance.UpdateInfoPanelProducer(this);
    }

    public virtual void SimulateHealth()
    {

    }

    public virtual void SimulateAge()
    {
        age += (Time.deltaTime / Environment.Instance.TimeScale);

        if (age >= lifeSpan)
        {
            Die(CauseOfDeath.Age);
        }

        if(health <= 0.0f)
        {
            Die(CauseOfDeath.HealthDepleated);
        }
    }

    protected virtual void Die(CauseOfDeath cause)
    {
        if (!dead)
        {
            Debug.Log(cause);
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
        if (_renderer)
        {
            _renderer.material.SetFloat("_FirstOutlineWidth", value);
        }
    }

    protected IEnumerator FadeOutRoutine()
    {
        if(_renderer)
        {
            _renderer.material.shader = Shader.Find("Transparent/Diffuse");

            while(_renderer.material.color.a > 0.0f)
            {
                float alpha = _renderer.material.color.a;
                Color newColor = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, Mathf.Lerp(alpha, 0.0f, Time.deltaTime * 0.5f));
                _renderer.material.color = newColor;
                yield return null;
            }
        }

        Destroy(gameObject);
    }
}
