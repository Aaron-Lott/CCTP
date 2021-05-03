using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class StartPopulationSlider : MonoBehaviour
{
    private Image sliderFill;
    private Slider slider;

    private float sliderRange;
    private float distFromMin;
    private float sliderPercentage;

    private Text countText;
    public int count;

    private GameDataController gameDataController;

    public bool isUnlocked = true;
    public bool isMoorishIdolSlider = false;
    public bool isButterflyFishSlider = false;
    public bool isWhitetipReefSharkSlider = false;

    private void Start()
    {
        gameDataController = FindObjectOfType<GameDataController>();
        slider = GetComponent<Slider>();
        sliderFill = slider.fillRect.GetComponent<Image>();
        countText = GetComponentInChildren<Text>();

        if (isMoorishIdolSlider && gameDataController.MoorishIdolIsUnlocked()) { isUnlocked = true; slider.value = 30; }
        if(isButterflyFishSlider && gameDataController.ButterflyFishIsUnlocked()) { isUnlocked = true; slider.value = 30; }
        if (isWhitetipReefSharkSlider && gameDataController.WhitetipReefSharkIsUnlocked()) { isUnlocked = true; slider.value = 4; }
    }

    private void Update()
    {
        if(isUnlocked)
        {
            distFromMin = (int)(slider.value - slider.minValue);
            sliderRange = slider.maxValue - slider.minValue;
            sliderPercentage = 100 * (distFromMin / sliderRange);

            count = (int)slider.value;

            if (countText) countText.text = count.ToString();

            if (sliderPercentage >= 90 || sliderPercentage <= 20)
            {
                sliderFill.color = Colors.PastelRed;
            }
            else if (sliderPercentage >= 80 || sliderPercentage <= 30)
            {
                sliderFill.color = Colors.PastelOrange;
            }
            else
            {
                sliderFill.color = Colors.PastelGreen;
            }
        }
        else
        {
            slider.value = 0;
        }
    }


}
