using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGrassSpawner : MonoBehaviour
{
    public static SeaGrassSpawner Instance;

    public LivingEntity seaGrass;

    public int amount = 256;
    public float scaleVaritation = 0.25f;
    public float spacingFactor = 6.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MassSpawnGrass();
    }

    public void MassSpawnGrass()
    {
        for (int i = 0; i < amount; i++)
        {
            float sqrRootAmount = Mathf.Sqrt(amount);
            float spacing = Random.Range(1.0f, spacingFactor);

            GameObject newObj = Instantiate(seaGrass.gameObject, new Vector3(Random.Range(-sqrRootAmount * spacing, sqrRootAmount * spacing),
                Environment.Instance.SeaBedPosition, Random.Range(-sqrRootAmount * spacing, sqrRootAmount * spacing)),
                Quaternion.Euler(Quaternion.identity.x, Random.Range(-180, 180), Quaternion.identity.z));

            StartCoroutine(Environment.Instance.PositionOnSeaBed(newObj.transform));

            newObj.transform.localScale = newObj.transform.localScale * Random.Range(1.0f - scaleVaritation, 1.0f + scaleVaritation);
        }
    }

    public void SpawnGrass()
    {
       GameObject grass = Instantiate(seaGrass.gameObject, new Vector3
            (Environment.Instance.GetRandomTarget().x, Environment.Instance.SeaBedPosition, Environment.Instance.GetRandomTarget().z), Quaternion.identity);

        StartCoroutine(Environment.Instance.PositionOnSeaBed(grass.transform));
    }
}
