using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject informationPanel = null;
    [SerializeField] private Text speciesAndName = null, scientificName = null, minLifeSpanText = null, maxLifeSpanText = null, factText = null;
    [SerializeField] private Slider ageSlider = null;
    [SerializeField] private Slider healthSlider = null;
    [SerializeField] private Image ageSliderFill = null, healthSliderFill = null, gender = null;
    [SerializeField] private Sprite maleSymbol = null, femaleSymbol = null;

    //COLORS
    private Color pastelBlue = new Color(0.682f, 0.776f, 0.812f);
    private Color pastelGreen = new Color(0.467f, 0.867f, 0.467f);
    private Color pastelRed = new Color(1.0f, 0.412f, 0.38f);

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

    public void UpdateInformationPanel(LivingEntity entity)
    {
        if (speciesAndName) speciesAndName.text = entity.RandomName != null ? entity.RandomName + " | " + entity.Species.ToString() : entity.Species.ToString();
        if (scientificName) scientificName.text = entity.ScientificName;
        if (ageSlider) ageSlider.value = entity.Age / entity.MaxLifeSpan;
        if (maxLifeSpanText) maxLifeSpanText.text = entity.MaxLifeSpan.ToString();
        if (healthSlider) healthSlider.value = entity.Health;
        if (factText) factText.text = entity.Fact;

        if(gender)
        {
            gender.enabled = true;
            if (entity.Gender == Gender.Male) gender.sprite = maleSymbol;
            else if (entity.Gender == Gender.Female) gender.sprite = femaleSymbol;
            else gender.enabled = false;

        }

        if(entity.Age >= entity.MinLifeSpan)
        {
            ageSliderFill.color = pastelRed;
        }

        if(entity.Health <= 0.25f)
        {
            healthSliderFill.color = pastelRed;
        }

        ageSlider.gameObject.SetActive(entity.SimulateAge());
        maxLifeSpanText.gameObject.SetActive(entity.SimulateAge());

        if (!entity.SimulateAge()) minLifeSpanText.text = "N/A";
        else minLifeSpanText.text = "0";

    }




}
