using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FreeCamera : MonoBehaviour
{
    [SerializeField] private float flySpeed = 10.0f;
    [SerializeField] private float rotationSensitivity = 2.0f;

    private void LateUpdate()
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
}
