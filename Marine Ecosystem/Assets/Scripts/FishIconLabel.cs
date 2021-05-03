using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishIconLabel : MonoBehaviour
{
    public Canvas canvas;
    public GameDataController gameDataController;

    public void SetLabelActive(string species)
    {
        GetComponentInChildren<Text>().text = species;

        float labelSizeMod = species.Length * 1.5f;

        GetComponent<RectTransform>().sizeDelta = new Vector2(10 * labelSizeMod, 50);

        gameObject.SetActive(true);
    }

    public void SetLabelActiveWhitetipReefShark(string species)
    {
        if(gameDataController.WhitetipReefSharkIsUnlocked())
        {
            SetLabelActive(species);
        }
        else
        {
            SetLabelActive("Locked");
        }
    }

    public void SetLabelActiveMoorishIdol(string species)
    {
        if (gameDataController.MoorishIdolIsUnlocked())
        {
            SetLabelActive(species);
        }
        else
        {
            SetLabelActive("Locked");
        }
    }

    public void SetLabelActiveButterflyFish(string species)
    {
        if (gameDataController.ButterflyFishIsUnlocked())
        {
            SetLabelActive(species);
        }
        else
        {
            SetLabelActive("Locked");
        }
    }

    public void SetLabelInactive()
    {
        gameObject.SetActive(false);

    }

    private void Start()
    {
        SetLabelInactive();
    }

    private void Update()
    {
       Vector2 pos;
       RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
        transform.position = canvas.transform.TransformPoint(pos);
    }
}
