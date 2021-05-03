﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FreeCamera), typeof(OrbitCamera))]
public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private FreeCamera freeCam;
    private OrbitCamera orbitCam;

    private LivingEntity selectedEntity;

    [HideInInspector]
    public bool orbitCamEnabled = false;

    public bool canMove = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        freeCam = GetComponent<FreeCamera>();
        orbitCam = GetComponent<OrbitCamera>();

        SetOrbitCamera(false);

        if(GameDataController.Instance != null)
            DisableCameraMovement(true);

    }

    private void LateUpdate()
    {
        if(canMove)
        {
            if (orbitCamEnabled && (Input.GetButtonDown("Fire2") || selectedEntity.Dead))
            {
                selectedEntity = null;

                SetOrbitCamera(false);
                orbitCamEnabled = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !orbitCamEnabled)
            {
                UIManager.Instance.ToggleYearUIAndCursor();
                freeCam.enabled = !UIManager.Instance.YearUIActive();
            }

            if (Environment.Instance != null)
                Environment.Instance.KeepInBounds(transform);
        }
    }

    private void SetOrbitCamera(bool orbitCamActive)
    {
        freeCam.enabled = !orbitCamActive;
        orbitCam.enabled = orbitCamActive;
    }

    public StarfishCollectable DetectStarfish()
    {
        float castDist = orbitCam.maxDistance;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, castDist))
        {
            StarfishCollectable starfish = hit.transform.GetComponent<StarfishCollectable>();

            return starfish;
        }

        return null;
    }

    public LivingEntity DetectEntity()
    {
        if(!UIManager.Instance.YearUIActive())
        {
            float castDist = orbitCam.maxDistance;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, castDist))
            {
                LivingEntity entity = hit.transform.GetComponent<LivingEntity>();

                if (entity && !entity.Dead)
                {
                    if (UIManager.Instance != null)
                        UIManager.Instance.SetInformationPanelActive(true);

                    if (!orbitCamEnabled && Input.GetButtonDown("Fire1"))
                    {
                        selectedEntity = entity;

                        SetOrbitCamera(true);
                        orbitCam.SetFocus(entity.transform, entity.orbitCamViewOffset);
                        orbitCamEnabled = true;
                    }

                    if (selectedEntity) return selectedEntity;

                    return entity;
                }
            }
        }

        if (orbitCamEnabled)
            return selectedEntity;

        if (UIManager.Instance != null)
            UIManager.Instance.SetInformationPanelActive(false);


        return null;
    }

    public void ActivateFreeCam(bool active)
    {
        freeCam.enabled = active;
    }

    public void DisableCameraMovement(bool disable)
    {
        canMove = !disable;
        freeCam.enabled = !disable;

        if(disable)
        {
            UIManager.Instance.ShowAndUnlockCursor();
        }
        else
        {
            UIManager.Instance.HideAndLockCursor();
        }
    }
}
