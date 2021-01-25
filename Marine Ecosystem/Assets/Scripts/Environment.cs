using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public static Environment Instance;
    public int TimeScale { get; } = 60;

    public int WorldMinX { get; } = -60;
    public int WorldMaxX { get; } = 60;

    public int WorldMinY { get; } = -18;
    public int WorldMaxY { get; } = -1;

    public int WorldMinZ { get; } = -70;
    public int WorldMaxZ { get; } = 40;

    [Range(2010, 2060)]
    public int currentYear = 2021;
    private int minYear = 2010, maxYear = 2060;

    private float millTonnesOfRubbish;

    [Range(23f, 40f)]
    public float seaTemperature = 28f;
    private float maxTemperature = 32f, minTemperature = 28f;

    public float SeaTemperature { get { return seaTemperature;  } }

    public Vector3 waveDirection = Vector3.zero;
    public Vector3 WaveDirection { get { return waveDirection;  } }

    public float waveForce = 0;
    public float WaveForce { get { return waveForce; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if(UIManager.Instance != null)
        {
            UIManager.Instance.SetUpYearSlider(currentYear, minYear, maxYear);
            UIManager.Instance.SetUpTempSlider(minTemperature, maxTemperature);
        }

        StartCoroutine(WaveRoutine());
    }

    public Vector3 GetWaterCurrent()
    {
        return waveDirection.normalized * waveForce;
    }

    private void Update()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateYear(ref currentYear);
            UIManager.Instance.UpdateTemperatureUI(seaTemperature, minTemperature, maxTemperature);
            UIManager.Instance.UpdatePollutionUI(millTonnesOfRubbish);
        }

        if(seaTemperature > maxTemperature)
        {
            seaTemperature = maxTemperature;
        }
        else if(seaTemperature < minTemperature)
        {
            seaTemperature = minTemperature;
        }

        seaTemperature = (currentYear - 2010) * 0.08f + minTemperature;

        //using data from:
        //https://ourworldindata.org/plastic-pollution

        millTonnesOfRubbish = Mathf.Max((7.905f * (currentYear - 2018)) + 15.81f, 0f);
    }

    public void MassInstaniateEntities(GameObject[] corals, int amount, Transform parent, float spacingFactor = 1.0f, float scaleFactor = 0.0f)
    {
        for (int i = 0; i < amount; i++)
        {
            float sqrRootAmount = Mathf.Sqrt(amount);
            float spacing = Random.Range(1.0f, spacingFactor);
            int randCoral = Random.Range(0, corals.Length);

            GameObject newObj = Instantiate(corals[randCoral], parent.position +
                new Vector3(Random.Range(-sqrRootAmount * spacing, sqrRootAmount * spacing), 0, Random.Range(-sqrRootAmount * spacing, sqrRootAmount * spacing)),
                Quaternion.Euler(Quaternion.identity.x, Random.Range(-180, 180), Quaternion.identity.z));

            StartCoroutine(PositionOnSeaBed(newObj.transform));

            newObj.transform.localScale = newObj.transform.localScale * Random.Range(1.0f - scaleFactor, 1.0f + scaleFactor);

            newObj.transform.parent = parent;
        }
    }


    public IEnumerator PositionOnSeaBed(Transform obj)
    {
        int layerMask = 1 << 11;

        while (!Physics.Raycast(obj.position, Vector3.down, 10f, layerMask))
        {
            obj.transform.position += new Vector3(0, 0.1f, 0);
            yield return null;
        }
    }

    private IEnumerator WaveRoutine()
    {
        float minTime = 5f, maxTime = 15f;

        while(true)
        {
            waveDirection = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            waveForce = Random.Range(0f, 3f);

            float time = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(time);
        }
    }

    public void KeepInBounds(Transform t)
    {
        if (t.position.x < WorldMinX)
        {
            t.position = new Vector3(WorldMinX, t.position.y, t.position.z);
        }
        else if (t.position.x > WorldMaxX)
        {
            t.position = new Vector3(WorldMaxX, t.position.y, t.position.z);
        }

        if (t.position.y < WorldMinY)
        {
            t.position = new Vector3(t.position.x, WorldMinY, t.position.z);
        }
        else if(t.position.y > WorldMaxY)
        {
            t.position = new Vector3(t.position.x, WorldMaxY, t.position.z);
        }

        if (t.position.z < WorldMinZ)
        {
            t.position = new Vector3(t.position.x, t.position.y, WorldMinZ);
        }
        else if (t.position.z > WorldMaxZ)
        {
            t.position = new Vector3(t.position.x, t.position.y, WorldMaxZ);
        }
    }

    public Vector3 GetRandomTarget()
    {
        int randX = Random.Range(WorldMinX, WorldMaxX);
        int randY = Random.Range(WorldMinY, WorldMaxY);
        int randZ = Random.Range(WorldMinZ, WorldMaxZ);

        return new Vector3(randX, randY, randZ);
    }
}

    

