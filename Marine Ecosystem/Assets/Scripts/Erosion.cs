using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Erosion : MonoBehaviour
{
    public GameObject[] rockTypes;
    public Transform[] spawnPositions;

    public Transform starSpawnPos;

    private bool rocksSpawned = false;

    private void SpawnRocks()
    {
        int amount = Random.Range(4, 8);

        for (int i = 0; i < amount; i++)
        {
           GameObject rock =  Instantiate(rockTypes[Random.Range(0, rockTypes.Length)], spawnPositions[i].position, Quaternion.identity);
            rock.transform.parent = transform;
        }
    }

    private void Update()
    {
        if((int)Environment.Instance.currentYear == 2024)
        {
            if(!rocksSpawned)
            {
                SpawnRocks();
                StarfishManager.Instance.SpawnStarfish(AchievementTypes.CRUMBLING_AWAY, new Vector3(-3.7f, -8.4f, 44.2f));
                rocksSpawned = true;
            }
        }
    }

}
