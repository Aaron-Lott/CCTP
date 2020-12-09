using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject informationPanel;
    [SerializeField] private Text nameText, speciesText, knownAsText, ageText, factText; 

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

    public void UpdateInformationPanel(string species, string age, string knownAs, string name = "", string fact = "")
    {
        if(speciesText) speciesText.text = species;
        if (ageText) ageText.text = age;
        if (knownAsText) knownAsText.text = knownAs;
        if(nameText) nameText.text = name;
        if (factText) factText.text = fact;
    }




}
