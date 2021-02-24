using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    public static Music Instance;

    public AudioMixer audioMixer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(!GameDataController.Instance.GetMusicIsOn())
        {
            StopPlaying();
        }

        audioMixer.SetFloat("volume", GameDataController.Instance.GetMasterVolume());
    }

    public void StopPlaying()
    {
        if (GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Stop();
    }

    public void StartPlaying()
    {
        if(!GetComponent<AudioSource>().isPlaying)
        GetComponent<AudioSource>().Play();
    }
}
