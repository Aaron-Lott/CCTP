using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float CohesionFactor = 1f;

    [Range(0f, 1f)]
    public float AlignmentFactor = 1f;

    [Range(0f, 1f)]
    public float SeperationFactor = 1f;

    public Vector3 GoalPosition { get; private set; }

    private List<Fish> allFishInSchool = new List<Fish>();

    private void Start()
    {
        //StartCoroutine(SetTargetPositionRoutine());
        PopulateFishInSchoolList();
    }

    private void Update()
    {
        if(Random.Range(0, 1000) < 5)
        {
            GoalPosition = Environment.Instance.GetRandomTarget();
        }
    }

    private IEnumerator SetTargetPositionRoutine()
    {
        while(transform.childCount > 0)
        {
            GoalPosition = Environment.Instance.GetRandomTarget();

            float randTime = Random.Range(5, 20);
            yield return new WaitForSeconds(randTime);
        }
    }

    private void PopulateFishInSchoolList()
    {
        foreach(Fish fish in transform.GetComponentsInChildren<Fish>())
        {
            allFishInSchool.Add(fish);
        }
    }

    public List<Fish> GetAllFishInSchool()
    {
        return allFishInSchool;
    }

    public void RemoveFishFromSchool(Fish fish)
    {
        allFishInSchool.Remove(fish);
    }
}
