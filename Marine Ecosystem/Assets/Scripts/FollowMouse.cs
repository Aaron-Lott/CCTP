using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Vector3 mousePosition;
    public float moveSpeed = 2f;

    private Animator anim;
    float startAnimSpeed;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        startAnimSpeed = anim.speed;
    }

    void Update()
    {
        Vector3 temp = Input.mousePosition;
        temp.z = 7f; 

        Vector3 targetPos = Camera.main.ScreenToWorldPoint(temp);

        if(Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            anim.speed = startAnimSpeed * 2;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
        else
        {
            anim.speed = startAnimSpeed;
        }


        if ((targetPos - transform.position) != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetPos - transform.position), 0.15f);
    }
    
}
