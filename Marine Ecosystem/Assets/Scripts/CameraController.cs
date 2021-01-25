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

    [HideInInspector]
    public bool orbitCamEnabled = false;

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
    }

    private void LateUpdate()
    {
        if (orbitCamEnabled && (Input.GetButtonDown("Fire2") || selectedEntity.Dead))
        {
            selectedEntity = null;

            SetOrbitCamera(false);
            orbitCamEnabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Tab) && !orbitCamEnabled)
        {
            UIManager.Instance.ToggleYearUI();
            freeCam.enabled = !UIManager.Instance.YearUIActive();
        }

        if (Environment.Instance != null)
        Environment.Instance.KeepInBounds(transform);
    }

    private void SetOrbitCamera(bool orbitCamActive)
    {
        freeCam.enabled = !orbitCamActive;
        orbitCam.enabled = orbitCamActive;
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

        if (UIManager.Instance != null)
            UIManager.Instance.SetInformationPanelActive(false);

        return null;
    }

    public void ActivateFreeCam(bool active)
    {
        freeCam.enabled = active;
    }
}
