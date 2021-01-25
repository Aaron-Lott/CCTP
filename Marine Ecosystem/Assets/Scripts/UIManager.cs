using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    //CUSORS
    [HideInInspector] public bool mouseCursorEnabled = false;
    [SerializeField] private Texture2D mouseCursor = null;

    //COLORS
    private Color pastelBlue = new Color(0.682f, 0.776f, 0.812f);
    private Color pastelGreen = new Color(0.467f, 0.867f, 0.467f);
    private Color pastelRed = new Color(1.0f, 0.412f, 0.38f);
    private Color pastelOrange = new Color(1.0f, 0.720f, 0.278f);
    private Color pastelYellow = new Color(0.992f, 0.992f, 0.588f);

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

    //THERMOMETER
    [SerializeField] private Slider temperatureSlider = null;
    [SerializeField] private Image thermometerUI = null;
    [SerializeField] private Text temperatureText = null;

    //POLLUTION     
    [SerializeField] private Text rubbishText = null;


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

        HideAndLockCursor();
    }

    private void Update()
    {   
    }

    public void ToggleYearUI()
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
            ageSliderFill.color = pastelRed;
        }
        else
        {
            ageSliderFill.color = pastelBlue;
        }

        if(consumer.Health <= 0.25f)
        {
            healthSliderFill.color = pastelRed;
        }
        else
        {
            healthSliderFill.color = pastelGreen;
        }

        if(consumer.Hunger >= consumer.CriticalHungerPercent)
        {
            hungerSliderFill.color = pastelRed;
        }
        else
        {
            hungerSliderFill.color = pastelYellow;
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
            healthSliderFill.color = pastelRed;
        }
        else
        {
            healthSliderFill.color = pastelGreen;
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
            thermometerUI.color = pastelRed;
            sliderImage.color = pastelRed;
            temperatureText.color = pastelRed;
        }
        else if(currentTemp >= maxTemp - 2 && currentTemp < maxTemp - 1)
        {
            thermometerUI.color = pastelOrange;
            sliderImage.color = pastelOrange;
            temperatureText.color = pastelOrange;
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

    public void UpdateYear(ref int currentYear)
    {
        currentYear = (int)yearSlider.value;
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

    public void UpdatePollutionUI(float tonnesOfRubbish)
    {
        tonnesOfRubbish = Mathf.Round(tonnesOfRubbish * 100f) / 100f;
        rubbishText.text = tonnesOfRubbish + " million tonnes";
    }
}
