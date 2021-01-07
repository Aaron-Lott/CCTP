using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnvironmentUtility 
{
    public static IEnumerator PositionOnSeaBed(Transform obj)
    {
        int layerMask = 1 << 11;

        while (!Physics.Raycast(obj.position, Vector3.down, 10f, layerMask))
        {
            obj.transform.position += new Vector3(0, 0.1f, 0);
            yield return null;
        }
    }
}
