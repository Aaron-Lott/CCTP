using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FreeCamera), typeof(OrbitCamera))]
public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    private FreeCamera freeCam;
    private OrbitCamera orbitCam;

    private LivingEntity selectedEntity;

    private bool orbitCamEnabled = false;

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

        HideAndLockCursor();
        SetOrbitCamera(false);
    }

    private void LateUpdate()
    {
        if (orbitCamEnabled && Input.GetButtonDown("Fire2"))
        {
            selectedEntity = null;

            SetOrbitCamera(false);
            orbitCamEnabled = false;
        }
    }

    private void SetOrbitCamera(bool orbitCamActive)
    {
        freeCam.enabled = !orbitCamActive;
        orbitCam.enabled = orbitCamActive;
    }

    void HideAndLockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void ShowAndUnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public LivingEntity DetectEntity()
    {
        float castDist = orbitCam.maxDistance;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, castDist))
        {
            LivingEntity entity = hit.transform.GetComponent<LivingEntity>();

            if (entity && !entity.Dead)
            {
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

        UIManager.Instance.SetInformationPanelActive(false);
        
        return null;
    } 
}
