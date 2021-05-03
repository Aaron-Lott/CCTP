using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfishManager : MonoBehaviour
{
    public static StarfishManager Instance;

    public StarfishCollectable[] starfishCollectables;

    [HideInInspector] public int correctAnswers = 0;

    private Dictionary<AchievementTypes, bool> starfishSpawned = new Dictionary<AchievementTypes, bool>();

    private void Awake()
    {
        if(Instance == null)
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
        foreach(AchievementTypes type in System.Enum.GetValues(typeof(AchievementTypes)))
        {
            starfishSpawned.Add(type, false);
        }

        SpawnStarfish(AchievementTypes.STARFISH_DETECTIVE, Environment.Instance.GetRandomTarget());
    }

    public void SpawnStarfish(AchievementTypes type, Vector3 position)
    {
        if(starfishSpawned[type] == false)
        {
            Instantiate(starfishCollectables[(int)type], position, Quaternion.identity);
            Debug.Log(type + ": " + position);
            starfishSpawned[type] = true;
        }
    }

    public void CorrectAnswer()
    {
        correctAnswers++;
    }
}
