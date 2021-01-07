using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WaterCollider : MonoBehaviour
{
    private BoxCollider boxCollider;
    private WaterPlaneGenerator waterSurface;

    private int waterDepth = 20;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        waterSurface = FindObjectOfType<WaterPlaneGenerator>();

        SetUpBoxCollider(waterSurface.size, waterSurface.size, waterDepth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Camera.main.gameObject)
        {
            if (waterSurface) waterSurface.FlipPlane(-1);
            PostProcessController.Instance.SetPostProcessProfile(PostProcessController.Instance.underWater);
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Camera.main.gameObject)
        {
            if(waterSurface) waterSurface.FlipPlane(1);
            PostProcessController.Instance.SetPostProcessProfile(PostProcessController.Instance.aboveWater);
        }

        //turns sea creature around if leaving area.

        Consumer consumer = other.gameObject.GetComponent<Consumer>();

        if (consumer)
        {
            consumer.moveDirection *= -1;
        }
    }*/

    private void SetUpBoxCollider(float sizeX, float sizeZ, float waterDepth)
    {
        boxCollider.size = new Vector3(sizeX, waterDepth, sizeZ);
        boxCollider.center = new Vector3(0, -waterDepth / 2, 0);
    }
}
