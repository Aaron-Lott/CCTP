using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class PostProcessController : MonoBehaviour
{
    private PostProcessVolume volume;

    public PostProcessProfile aboveWater, underWater;

    public static PostProcessController Instance;

    private ColorGrading colorGrading = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out colorGrading);

        //StartCoroutine(GreenRoutine());
    }

    public void SetPostProcessProfile(PostProcessProfile profile)
    {
        volume.profile = profile;
    }

    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.Q))
        {
            colorGrading.mixerBlueOutGreenIn.value = Mathf.Lerp(0, -200, Time.deltaTime);
        }
    }

    private IEnumerator GreenRoutine()
    {
        while(!Input.GetKeyDown(KeyCode.Q))
        {
            yield return null;
        }

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * 0.05f)
        {
            colorGrading.mixerBlueOutGreenIn.value = Mathf.Lerp(0, -200, t);
            yield return null;
        }
    }
}
