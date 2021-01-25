using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
    private Transform focus = default;

    [SerializeField, Range(0.5f, 4f)]
    public float distance = 2f;

    [HideInInspector] public float maxDistance = 5f;
    private float minDistance = 1f;

    [SerializeField, Range(1f, 360f)]
    private float rotationSpeed = 120f;

    [SerializeField, Range(1f, 4f)]
    private float zoomSpeed = 2f;

    Vector2 orbitAngles = new Vector2(45f, 0f);

    private void LateUpdate()
    {
        Vector3 focusPoint = focus.position;
        Quaternion lookRotation = Quaternion.Euler(orbitAngles);
        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDirection * distance;
        transform.SetPositionAndRotation(lookPosition, lookRotation);

        ManualRotation();
        ManualDistance();
    }

    private void ManualRotation()
    {
        Vector2 input = new Vector2(
            Input.GetAxis("Mouse Y"),
            Input.GetAxis("Mouse X")
        );
        const float e = 0.001f;
        if (input.x < -e || input.x > e || input.y < -e || input.y > e)
        {
            orbitAngles -= rotationSpeed * Time.unscaledDeltaTime * input;
        }
    }

    private void ManualDistance()
    {
        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if(distance <= minDistance)
        {
            distance = minDistance;
        }
        else if(distance >= maxDistance)
        {
            distance = maxDistance;
        }
    }



    public void SetFocus(Transform f, Vector3 offset)
    {
        focus = f;      
    }

}
