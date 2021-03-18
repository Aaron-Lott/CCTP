using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction : MonoBehaviour
{
    public bool startActive = false;

    private void Start()
    {
        gameObject.SetActive(startActive);
    }

    public void NextInstruction(Instruction nextInstruction)
    {
        gameObject.SetActive(false);
        nextInstruction.gameObject.SetActive(true);
    }

    public void FinalInstruction()
    {
        gameObject.SetActive(false);
        CameraController.Instance.DisableCameraMovement(false);
    }

    public void ShowYearInstruction(Instruction nextInstruction)
    {
        NextInstruction(nextInstruction);
        UIManager.Instance.SetYearUIActive(true);
    }

    public void ShowControlsInstruction(Instruction nextInstruction)
    {
        UIManager.Instance.SetYearUIActive(false);
        NextInstruction(nextInstruction);
    }
}
