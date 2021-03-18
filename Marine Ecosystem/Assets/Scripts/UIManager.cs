using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public bool lockCursorOnAwake = true;

    //CUSORS
    [HideInInspector] public bool mouseCursorEnabled = false;
    [SerializeField] private Texture2D mouseCursor = null;

    //INFO PANEL
    [SerializeField] private GameObject informationPanel = null;
    [SerializeField] private Text speciesAndName = null, scientificName = null, minLifeSpanText = null, maxLifeSpanText = null, actionText = null, hungerText = null;
    [SerializeField] private Slider ageSlider = null, healthSlider = null, hungerSlider = null;
    [SerializeField] private Image ageSliderFill = null, healthSliderFill = null, hungerSliderFill = null, gender = null;
    [SerializeField] private Sprite maleSymbol = null, femaleSymbol = null;

    //YEAR
    [SerializeField] private Animator yearUIAnim;
    [SerializeField] private Text yearText = null;
    [SerializeField] private Slider yearSlider = null;

    private bool yearUIActive = false;

    //ACHIEVMENT UI
    [SerializeField] private Text achivementTitle = null, achievementText = null;
    [SerializeField] private Image medalImage = null, achievementArt = null;

    //THERMOMETER
    [SerializeField] private Slider temperatureSlider = null;
    [SerializeField] private Image thermometerUI = null;
    [SerializeField] private Text temperatureText = null;

    //POLLUTION     
    [SerializeField] private Text rubbishText = null;
    [SerializeField] private Image chemicalPollutionUI = null;

    //STARFISH 
    [SerializeField] private Text starfishCount;


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

        Vector2 cursorOffset = new Vector2(mouseCursor.width / 2, mouseCursor.height / 2);
        Cursor.SetCursor(mouseCursor, Vector3.zero, CursorMode.Auto);

        if(lockCursorOnAwake && !GameDataController.Instance.GetInstructionsEnabled())
        HideAndLockCursor();
    }

    private void Start()
    {
        UpdateStarfishCount(GameDataController.Instance.GetAchievementCount());
    }

    public void ToggleYearUIAndCursor()
    {
        yearUIActive = !yearUIActive;
        yearUIAnim.SetBool("activated", yearUIActive);

        if(yearUIActive)
        {
            ShowAndUnlockCursor();
        }
        else
        {
            HideAndLockCursor();
        }
    }

    public bool YearUIActive()
    {
        return yearUIActive;
    }
    
    public void SetYearUIActive(bool active)
    {
        yearUIActive = active;
        yearUIAnim.SetBool("activated", yearUIActive);
    }

    public void SetInformationPanelActive(bool active)
    {
        informationPanel.SetActive(active);
    }

    public void UpdateInfoPanelConsumer(Consumer consumer)
    {
        ageSlider.gameObject.SetActive(true);
        hungerSlider.gameObject.SetActive(true);

        speciesAndName.text = consumer.RandomName + " | " + consumer.Species.ToString();
        scientificName.text = consumer.ScientificName;
        ageSlider.value = consumer.Age / consumer.MaxLifeSpan;
        maxLifeSpanText.text = consumer.MaxLifeSpan.ToString();
        healthSlider.value = consumer.Health;
        hungerSlider.value = consumer.Hunger;
        hungerText.text = "";
        actionText.text = consumer.CurrentAction.ToString();

        gender.enabled = true;
        if (consumer.Gender == Gender.Male) gender.sprite = maleSymbol;
        else if (consumer.Gender == Gender.Female) gender.sprite = femaleSymbol;


        minLifeSpanText.text = "0";

        if (consumer.Age >= consumer.MinLifeSpan)
        {
            ageSliderFill.color = Colors.PastelRed;
        }
        else
        {
            ageSliderFill.color = Colors.PastelBlue;
        }

        if(consumer.Health <= 0.25f)
        {
            healthSliderFill.color = Colors.PastelRed;
        }
        else
        {
            healthSliderFill.color = Colors.PastelGreen;
        }

        if(consumer.Hunger >= consumer.CriticalHungerPercent)
        {
            hungerSliderFill.color = Colors.PastelRed;
        }
        else
        {
            hungerSliderFill.color = Colors.PastelYellow;
        }

    }

    public void UpdateInfoPanelProducer(LivingEntity producer)
    {
        speciesAndName.text = producer.Species.ToString();
        scientificName.text = producer.ScientificName;
        healthSlider.value = producer.Health;
        ageSlider.gameObject.SetActive(false);
        hungerSlider.gameObject.SetActive(false);

        maxLifeSpanText.text = "";
        minLifeSpanText.text = "N/A";
        hungerText.text = "N/A";
        actionText.text = "N/A";

        gender.enabled = false;

        if (producer.Health <= 0.25f)
        {
            healthSliderFill.color = Colors.PastelRed;
        }
        else
        {
            healthSliderFill.color = Colors.PastelGreen;
        }
    }

    public void SetUpTempSlider(float minTemp, float maxTemp)
    {
        temperatureSlider.minValue = minTemp;
        temperatureSlider.maxValue = maxTemp;
    }


    public void UpdateTemperatureUI(float currentTemp, float minTemp, float maxTemp)
    {
        float temp = Mathf.Round(currentTemp * 10f) / 10f;
        temperatureSlider.value = temp;
        temperatureText.text = temp + "°C";

        Image sliderImage = temperatureSlider.GetComponentInChildren<Image>();

        if(currentTemp >= maxTemp - 1)
        {
            thermometerUI.color = Colors.PastelRed;
            sliderImage.color = Colors.PastelRed;
            temperatureText.color = Colors.PastelRed;
        }
        else if(currentTemp >= maxTemp - 2 && currentTemp < maxTemp - 1)
        {
            thermometerUI.color = Colors.PastelOrange;
            sliderImage.color = Colors.PastelOrange;
            temperatureText.color = Colors.PastelOrange;
        }
        else
        {
            thermometerUI.color =  Color.white;
            sliderImage.color = Color.white;
            temperatureText.color = Color.white;
        }
    }

    public void SetUpYearSlider(int currentYear, int minYear, int maxYear)
    {
        yearSlider.wholeNumbers = true;
        yearSlider.minValue = minYear;
        yearSlider.maxValue = maxYear;
        yearSlider.value = currentYear;
    }

    public void UpdateYear(int currentYear)
    {
        //currentYear = (int)yearSlider.value;
        yearSlider.value = currentYear;
        yearText.text = currentYear.ToString();
    }

    public void HideAndLockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowAndUnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UpdatePollutionUI(float tonnesOfRubbish, float chemicalLevels)
    {
        tonnesOfRubbish = Mathf.Round(tonnesOfRubbish * 100f) / 100f;
        rubbishText.text = tonnesOfRubbish + " million tonnes";

        if (chemicalLevels > 0.4f && chemicalLevels < 0.8f)
        {
            chemicalPollutionUI.color = Colors.PastelOrange;
        }
        else if(chemicalLevels > 0.8f)
        {
            chemicalPollutionUI.color = Colors.PastelRed;
        }

    }

    public void SetAchievementPanel(AchievementSettings settings)
    {
        achivementTitle.text = settings.achievementTitle;
        achievementText.text = settings.achievementText;

        medalImage.sprite = settings.medalImage;
        achievementArt.sprite = settings.achievementArt;
    }

    public bool IsCursorShownAndUnlocked()
    {
        if(Cursor.lockState == CursorLockMode.Locked || !Cursor.visible)
        {
            return false;
        }

        return true;
    }

    public void UpdateStarfishCount(int starfish)
    {
        if(starfishCount)
        starfishCount.text = starfish.ToString();
    }
}
