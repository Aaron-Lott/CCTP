using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject informationPanel = null;
    [SerializeField] private Text speciesAndName = null, scientificName = null, minLifeSpanText = null, maxLifeSpanText = null, actionText = null, hungerText = null;
    [SerializeField] private Slider ageSlider = null, healthSlider = null, hungerSlider = null;
    [SerializeField] private Image ageSliderFill = null, healthSliderFill = null, hungerSliderFill = null, gender = null;
    [SerializeField] private Sprite maleSymbol = null, femaleSymbol = null;

    //COLORS
    private Color pastelBlue = new Color(0.682f, 0.776f, 0.812f);
    private Color pastelGreen = new Color(0.467f, 0.867f, 0.467f);
    private Color pastelRed = new Color(1.0f, 0.412f, 0.38f);
    private Color pastelYellow = new Color(0.992f, 0.992f, 0.588f);

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

    private void Update()
    {
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




}
