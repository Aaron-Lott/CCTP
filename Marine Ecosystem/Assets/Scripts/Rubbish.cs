using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubbish : Pollution
{
    Renderer _renderer;

    [Range(0.0f, 1.0f)]
    public float buoyancy = 0.5f;

    private float startBuoyancy;

    [Range(0.0f, 0.1f)]
    public float buoyancyVariation = 0.1f;

    private float minBuoyancyTime = 2, maxBuoyancyTime = 12;


    protected override void Init()
    {
        base.Init();

        startBuoyancy = buoyancy;
    }

    protected override void Update()
    {
        if(gameObject.activeSelf)
        {
            float yPos = Mathf.Lerp(Environment.Instance.WorldMinY, Environment.Instance.WorldMaxY, buoyancy);
            Vector3 waterCurrent = Environment.Instance.GetWaterCurrent();

            transform.position = new Vector3(transform.position.x, yPos, transform.position.z) + waterCurrent * Time.deltaTime;

            transform.Rotate(waterCurrent.z, waterCurrent.y, -waterCurrent.x);
        }
    }

    protected IEnumerator BuoyancyRoutine()
    {
        while(gameObject.activeSelf)
        {
            float elapsedTime = 0f;

            float time = Random.Range(minBuoyancyTime, maxBuoyancyTime);

            float newBuoyancy = buoyancy;

            float targetBuoyancy = Random.Range(startBuoyancy - buoyancyVariation, startBuoyancy + buoyancyVariation);

            while (elapsedTime < time)
            {
                buoyancy = Mathf.Lerp(newBuoyancy, targetBuoyancy, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return null;
        }
    }

    protected IEnumerator FadeRoutine(Material mat, float from, float to)
    {
        mat.shader = Shader.Find("Transparent/Diffuse");

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * 0.25f)
        {
            Color newColor = new Color(mat.color.r, mat.color.g, mat.color.b, Mathf.Lerp(from, to, t));
            mat.color = newColor;
            yield return null;
        }

        if (to == 0.0f)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (buoyancyVariation > 0)
            StartCoroutine(BuoyancyRoutine());

        Renderer _renderer = GetComponentInChildren<Renderer>();

        if (_renderer)
        {
            foreach (Material mat in _renderer.materials)
            {
                StartCoroutine(FadeRoutine(mat, 0.0f, 1.0f));
            }
        }
    }

    private void Disable()
    {
        Renderer _renderer = GetComponentInChildren<Renderer>();

        if (_renderer)
        {
            foreach (Material mat in _renderer.materials)
            {
                StartCoroutine(FadeRoutine(mat, 1.0f, 0.0f));
            }
        }

            StopCoroutine(BuoyancyRoutine());
    }
}
