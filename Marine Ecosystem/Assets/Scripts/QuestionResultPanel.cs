using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionResultPanel : MonoBehaviour
{
    private float waitTime = 1;

    private void OnEnable()
    {
        StartCoroutine(DisableRoutine());
    }

    IEnumerator DisableRoutine()
    {
        yield return new WaitForSeconds(2);

        gameObject.SetActive(false);
    }
}
