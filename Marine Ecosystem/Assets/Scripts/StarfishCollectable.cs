using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfishCollectable : MonoBehaviour
{
    public float rotationSpeed = 100;
    protected Renderer _renderer;

    private bool discovered = false;

    public AchievementTypes achievementType;

    private StarfishPanel[] starfishPanels;

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        starfishPanels = FindObjectsOfType<StarfishPanel>();
        starfishPanels = Resources.FindObjectsOfTypeAll<StarfishPanel>();
    }


    private void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        if(!discovered)
        {
            if (CameraController.Instance.DetectStarfish() == this)
            {
                SetShaderOutline(0.04f);

                if (Input.GetButtonDown("Fire1"))
                {
                    GameDataController.Instance.UnlockAchievement(achievementType);
                    CameraController.Instance.DisableCameraMovement(true);
                    UIManager.Instance.UpdateStarfishCount(GameDataController.Instance.GetAchievementCount());
                    SetStarfishPanelActive();
                    StartCoroutine(FadeOutRoutine());
                    discovered = true;
                }
            }
            else
            {
                SetShaderOutline(0.0f);
            }
        }
    }

    private void SetStarfishPanelActive()
    {
       foreach(StarfishPanel panel in starfishPanels)
        {
            if(panel.achievementType == achievementType)
            {
                panel.gameObject.SetActive(true);
            }
        }
    }

    public void SetShaderOutline(float value)
    {
        if (_renderer)
        {
            _renderer.material.SetFloat("_FirstOutlineWidth", value);
        }
    }

    protected IEnumerator FadeOutRoutine()
    {
        if (_renderer)
        {
            _renderer.material.shader = Shader.Find("Transparent/Diffuse");
            float startAlpha = _renderer.material.color.a;

            for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * 0.5f)
            {
                Color newColor = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, Mathf.Lerp(startAlpha, 0.0f, t));
                _renderer.material.color = newColor;
                yield return null;
            }
        }

        Destroy(gameObject);
    }
}
