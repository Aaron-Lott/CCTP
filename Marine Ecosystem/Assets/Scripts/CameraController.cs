using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float flySpeed = 10.0f;
    [SerializeField] private float rotationSensitivity = 2.0f;

    void Start()
    {
        HideAndLockCursor();
    }

    void LateUpdate()
    {
        transform.position += MoveVector(flySpeed);

        transform.eulerAngles += RotationVectorMouse(rotationSensitivity);
    }

    private Vector3 MoveVector(float speed)
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        return (transform.forward * moveVertical + transform.right * moveHorizontal) * speed * Time.deltaTime;
    }

    private Vector3 RotationVectorMouse(float sensitivity)
    {
        float rotationHorizontal = Input.GetAxis("Mouse X");
        float rotationVertical = Input.GetAxis("Mouse Y");

        return new Vector3(-rotationVertical * rotationSensitivity, rotationHorizontal * rotationSensitivity, 0.0f);
    }

    private Vector3 RotationVectorJoyStick(float sensitivity)
    {
        float rotationHorizontal = Input.GetAxis("RightStick X");
        float rotationVertical = Input.GetAxis("RightStick Y");

        return new Vector3(-rotationVertical * rotationSensitivity, rotationHorizontal * rotationSensitivity, 0.0f);
    }

    private bool IsAboveWaterSurface()
    {
        //gets the Water Surface Layer.
        int waterSurfaceLayerMask = 1 << 9;

        return Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, waterSurfaceLayerMask);
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
}
