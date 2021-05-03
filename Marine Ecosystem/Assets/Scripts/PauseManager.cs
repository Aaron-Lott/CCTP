using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    public GameObject pausePanel;

    private void Start()
    {
        pausePanel.SetActive(false);

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(CameraController.Instance.canMove)
            {
                pausePanel.SetActive(true);
                CameraController.Instance.DisableCameraMovement(true);
            }
        }
    }
}
