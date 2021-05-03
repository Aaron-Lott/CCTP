﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public static Environment Instance;
    public int TimeScale { get; } = 15;

    public int WorldMinX { get; } = -50;
    public int WorldMaxX { get; } = 50;

    public int WorldMinY { get; } = -15;
    public int WorldMaxY { get; } = -1;

    public int WorldMinZ { get; } = -60;
    public int WorldMaxZ { get; } = 45;

    public float SeaBedPosition { get; } = -22;

    [Range(2010, 2060)]
    public float currentYear = 2021;
    private int minYear = 2010, maxYear = 2060;

    [Range(1, 2)]
    private int timeModifier = 1;

    public float MillTonnesOfRubbish { get; private set; }
    public float MaxRubbish { get; private set; }


    public float ChemicalPollutionLevels { get; private set; }

    [Range(28f, 32f)]
    private float seaTemperature = 28f;
    private float maxTemperature = 32f, minTemperature = 28f;

    public float SeaTemperature { get { return seaTemperature;  } }

    public Vector3 waveDirection = Vector3.zero;
    public Vector3 WaveDirection { get { return waveDirection;  } }

    public float waveForce = 0;
    public float WaveForce { get { return waveForce; } }

    public Dictionary<Species, int> EntityPopulations = new Dictionary<Species, int>();

    public List<GameObject> SpeciesContainers = new List<GameObject>();

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

        InstaniateSpeciesContainers();
        PopulateEntityPopulationDictionaires();
    }

    private void Start()
    {
        if(UIManager.Instance != null)
        {
            UIManager.Instance.SetUpYearSlider((int)currentYear, minYear, maxYear);
            UIManager.Instance.SetUpTempSlider(minTemperature, maxTemperature);
        }

        currentYear = minYear;
        MaxRubbish = CalculateRubbishForYear(maxYear);

        StartCoroutine(WaveRoutine());
    }

    private float CalculateRubbishForYear(float year)
    {
        return Mathf.Max((7.905f * ((int)year - 2018)) + 15.81f, 0);
    }

    public Vector3 GetWaterCurrent()
    {
        return waveDirection.normalized * waveForce;
    }

    private void Update()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateYear((int)currentYear);
            UIManager.Instance.UpdateTemperatureUI(seaTemperature, minTemperature, maxTemperature);
            UIManager.Instance.UpdatePollutionUI(MillTonnesOfRubbish, ChemicalPollutionLevels);
        }

        if(seaTemperature > maxTemperature)
        {
            seaTemperature = maxTemperature;
        }
        else if(seaTemperature < minTemperature)
        {
            seaTemperature = minTemperature;
        }

        if(currentYear < maxYear)
        currentYear += (Time.deltaTime / TimeScale) * timeModifier;

        seaTemperature = (currentYear - minYear) * 0.08f + minTemperature;

        if(currentYear > 2020)
        ChemicalPollutionLevels = (currentYear - 2020) / (maxYear - 2020);

        //using data from:
        //https://ourworldindata.org/plastic-pollution

        MillTonnesOfRubbish = CalculateRubbishForYear(currentYear);

        if (MillTonnesOfRubbish >= 100)
        {
            StarfishManager.Instance.SpawnStarfish(AchievementTypes.THIS_MILESTONE_IS_GARBAGE, GetRandomTarget());
        }

        if (MillTonnesOfRubbish >= 200)
        {
            StarfishManager.Instance.SpawnStarfish(AchievementTypes.PLASTIC_GALAXY, GetRandomTarget());
        }

        if ((int)currentYear == maxYear)
        {
            UIManager.Instance.SetFinalMessage();
            StarfishManager.Instance.SpawnStarfish(AchievementTypes.ITS_NOT_TOO_LATE, GetRandomTarget());
        }
        else if((int)currentYear == 2048)
        {
            StarfishManager.Instance.SpawnStarfish(AchievementTypes.TWENTY_FOURTY_EIGHT, GetRandomTarget());
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

    public IEnumerator PositionOnSeaBed(Transform obj)
    {
        int layerMask = 1 << 11;

        while (!Physics.Raycast(obj.position, Vector3.down, 10f, layerMask))
        {
            obj.transform.position += new Vector3(0, 0.1f, 0);
            yield return null;
        }
    }

    private void InstaniateSpeciesContainers()
    {
        for(int i = 0; i < System.Enum.GetValues(typeof(Species)).Length; i++)
        {
            GameObject container = new GameObject(((Species)i).ToString() + " - Container");
            SpeciesContainers.Add(container);
        }
    }

    public void IncreaseEntityPopulation(Species species)
    {
        EntityPopulations[species] = EntityPopulations[species] += 1;
    }

    public void DecreaseEntityPopulation(Species species)
    {
        EntityPopulations[species] = EntityPopulations[species] -= 1;
    }

    public void PopulateEntityPopulationDictionaires()
    {
        foreach(Species species in System.Enum.GetValues(typeof(Species)))
        {
            EntityPopulations.Add(species, 0);
        }
    }

    public void PopulateSpeciesContainers(LivingEntity entity)
    {
        entity.transform.parent = SpeciesContainers[(int)entity.Species].transform;
    }

    public void AddBoidManagerToFishContainters(Fish fish)
    {
        if(!SpeciesContainers[(int)fish.Species].GetComponent<BoidManager>())
        {
            SpeciesContainers[(int)fish.Species].AddComponent<BoidManager>();
        }
    }

    public int GetEntityPopulation(Species species)
    {
        return EntityPopulations[species];
    }

    public bool IsInsideReef(Transform go)
    {
        return go.transform.position.x < WorldMaxX && go.transform.position.y < WorldMaxY &&
            go.transform.position.x > WorldMinX && go.transform.position.y > WorldMinY &&
            go.transform.position.z < WorldMaxZ && go.transform.position.z > WorldMinZ;
    }

    public Vector3 GetReefCenter()
    {
        return new Vector3((WorldMinX + WorldMaxX) / 2, (WorldMinY + WorldMaxY) / 2, (WorldMinZ + WorldMaxZ) / 2);
    }

    public void ToggleSpeedModifier(bool speed)
    {
        if(speed)
        {
            timeModifier = 2;
        }
        else
        {
            timeModifier = 1;
        }
    }
}

    

