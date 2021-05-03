using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbishController : MonoBehaviour
{
    public Rubbish[] rubbishPrefabs;

    private int maxAmount = 60;

    private List<Rubbish> rubbish = new List<Rubbish>();

    private void Start()
    {
        for(int i = 0; i < maxAmount; i++)
        {
            int randNum = Random.Range(0, rubbishPrefabs.Length);
            Rubbish rub = Instantiate(rubbishPrefabs[randNum], Environment.Instance.GetRandomTarget(), Quaternion.identity);
            rub.transform.parent = transform;
            rub.gameObject.SetActive(false);
            rubbish.Add(rub);
        }
    }

    private void Update()
    {
        int amount = (int)((Environment.Instance.MillTonnesOfRubbish / Environment.Instance.MaxRubbish) * maxAmount);

        for(int i = 0; i < rubbish.Count; i++)
        {
            if(i < amount)
            {
                if(!rubbish[i].gameObject.activeInHierarchy)
                rubbish[i].gameObject.SetActive(true);
            }
        }
    }

}
