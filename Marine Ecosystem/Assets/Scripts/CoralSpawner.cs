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
        MassInstaniateEntities(corals, amount, transform, spacingFactor, scaleVaritation);
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
}

