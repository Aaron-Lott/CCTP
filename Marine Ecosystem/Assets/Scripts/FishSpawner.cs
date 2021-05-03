using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public Consumer clownFish, butterflyFish, yellowTang, parrotFish, blacktipReefShark, moorishIdol, damselfish, whitetipReefShark;

    private void Start()
    {
        SpawnFish(clownFish, StartPopulationValues.Clownfish);
        SpawnFish(butterflyFish, StartPopulationValues.Butterflyfish);
        SpawnFish(yellowTang, StartPopulationValues.YellowTang);
        SpawnFish(parrotFish, StartPopulationValues.Parrotfish);
        SpawnFish(blacktipReefShark, StartPopulationValues.BlacktipReefShark);
        SpawnFish(moorishIdol, StartPopulationValues.MoorishIdol);
        SpawnFish(damselfish, StartPopulationValues.Damselfish);
        SpawnFish(whitetipReefShark, StartPopulationValues.WhitetipReefShark);
    }

    public void SpawnFish(Consumer fishPrefab, int population)
    {
        Vector3 randPosition = Environment.Instance.GetRandomTarget();

        for (int i = 0; i < population; i++)
        {
            Consumer fish = Instantiate(fishPrefab, randPosition, Quaternion.identity);
            
            if(i % 2 == 0)
            {
                fish.Gender = Gender.Female;
            }
            else
            {
                fish.Gender = Gender.Male;
            }
        }
    }
}
