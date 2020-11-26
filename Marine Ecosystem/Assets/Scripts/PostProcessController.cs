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
    }

    public void SetPostProcessProfile(PostProcessProfile profile)
    {
        volume.profile = profile;
    }
}
