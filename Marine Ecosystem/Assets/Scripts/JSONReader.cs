using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONReader: MonoBehaviour
{
    public TextAsset livingEntitiesData;
    public TextAsset namesFile;

    public LivingEntityData GetLivingEntityData(Species species)
    {
        LivingEntitiesData dataInJson = JsonUtility.FromJson<LivingEntitiesData>(livingEntitiesData.text);
        return dataInJson.livingEntities[(int)species];
    }

    public string GetRandomName()
    {
        string[] contents = namesFile.text.Split("\n"[0]);
        int random = Random.Range(1, contents.Length);

        return contents[random];
    }
}

[System.Serializable]
public class LivingEntityData
{
    public string species;
    public string knownAs;
    public string fact;
    public string lifeSpan;
}

[System.Serializable]
public class LivingEntitiesData
{
    public LivingEntityData[] livingEntities;
}