using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralSpawner : MonoBehaviour
{
    public int amount = 20;

    public GameObject[] corals;

    public float scaleVaritation = 0.25f;

    public float spacingFactor = 6.0f;

    private void Start()
    {
        MassInstaniateCoral(corals, amount, transform, spacingFactor, scaleVaritation);
    }

    public void MassInstaniateCoral(GameObject[] corals, int amount, Transform parent, float spacingFactor = 1.0f, float scaleFactor = 0.0f)
    {
        for (int i = 0; i < amount; i++)
        {
            float sqrRootAmount = Mathf.Sqrt(amount);
            float spacing = Random.Range(1.0f, spacingFactor);
            int randCoral = Random.Range(0, corals.Length);

            GameObject newObj = Instantiate(corals[randCoral], parent.position +
                new Vector3(Random.Range(-sqrRootAmount * spacing, sqrRootAmount * spacing), 0, Random.Range(-sqrRootAmount * spacing, sqrRootAmount * spacing)),
                Quaternion.Euler(Quaternion.identity.x, Random.Range(-180, 180), Quaternion.identity.z));

            StartCoroutine(Environment.Instance.PositionOnSeaBed(newObj.transform));

            newObj.transform.localScale = newObj.transform.localScale * Random.Range(1.0f - scaleFactor, 1.0f + scaleFactor);
        }
    }
}

