using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider audioSlider;
    public Dropdown graphicsDropdown;

    public Dropdown resolutionDropdown;

    public Toggle musicOnToggle;
    public Toggle musicOffToggle;

    private Resolution[] resolutions;

    private Color pastelGreen = new Color(0.467f, 0.867f, 0.467f);
    private Color pastelRed = new Color(1.0f, 0.412f, 0.38f);
    private Color grey = new Color(0.486f, 0.486f, 0.486f);

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResIndex = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;

        resolutionDropdown.RefreshShownValue();

        audioSlider.value = GameDataController.Instance.GetMasterVolume();
        SetVolume(audioSlider.value);

        graphicsDropdown.value = GameDataController.Instance.GetGraphicsQuality();
        SetGraphicsQuality(graphicsDropdown.value);

        SetOnMusicToggle(GameDataController.Instance.GetMusicIsOn());
    }


    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        GameDataController.Instance.SetMasterVolume(volume);
    }

    public void SetGraphicsQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        GameDataController.Instance.SetGraphicsQuality(index);
    }

    private void Update()
    {
        SetVolume(audioSlider.value);
    }

    public void TurnMusicOn()
    {
        GameDataController.Instance.SetMusicIsOn(true);

        SetOnMusicToggle(true);

        if (Music.Instance != null)
            Music.Instance.StartPlaying();
    }

    public void TurnMusicOff()
    {
        GameDataController.Instance.SetMusicIsOn(false);

        SetOnMusicToggle(false);

        if (Music.Instance != null)
            Music.Instance.StopPlaying();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetOnMusicToggle(bool on)
    {
        if(on)
        {
            musicOnToggle.GetComponent<Text>().color = pastelGreen;
            musicOffToggle.GetComponent<Text>().color = grey;
        }
        else
        {
            musicOffToggle.GetComponent<Text>().color = pastelRed;
            musicOnToggle.GetComponent<Text>().color = grey;
        }
    }
}
