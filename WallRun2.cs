﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun2 : MonoBehaviour
{
    // This Script and the WallRun script are meant for the same purpose!!!
    [SerializeField]private float upForce;
    [SerializeField]private float slideForce;
    [SerializeField]private Rigidbody rb;
    [SerializeField]private Transform head;
    [SerializeField]private Transform cam;
    private bool isLeft;
    private bool isRight;
    private float distFromLeft;
    private float distFromRight;

    private void Awake ()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void wallChecker ()
    {
        RaycastHit leftWall;
        RaycastHit rightWall;

        if (Physics.Raycast(head.position, head.right, out rightWall))
        {
            distFromRight = Vector3.Distance(head.position, rightWall.point);
            if (distFromRight <= 3f)
            {
                isRight = true;
                isLeft = false;
            }
        }
        if (Physics.Raycast(head.position, -head.right, out leftWall))
        {
            distFromLeft = Vector3.Distance(head.position, leftWall.point);
            if (distFromLeft <= 3f)
            {
                isRight = false;
                isLeft = true;
            }
        }
    }

    private void Update ()
    {
        wallChecker();
    }

    private void OnCollisionEnter (Collider collider)
    {
        if (collider.transform.CompareTag("RunnableWall"))
        {
            rb.useGravity = false;

            if (isRight)
            {
                cam.localEulerAngles = new Vector3(0, 0, 10f);
            }
            if (isLeft)
            {
                cam.localEulerAngles = new Vector3(0, 0, -10f);
            }
        }
    }

    private void OnCollisionStay (Collider collider)
    {
        if (collider.transform.CompareTag("RunnableWall"))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (isLeft)
                {
                    rb.AddForce(Vector3.up * upForce * Time.deltaTime);
                    rb.AddForce(head.right * slideForce * Time.deltaTime);
                }
                if (isRight)
                {
                    rb.AddForce(Vector3.up * upForce * Time.deltaTime);
                    rb.AddForce(-head.right * slideForce * Time.deltaTime);
                }
            }
        }
    }

    private void OnCollisionExit (Collider collider)
    {
        if (collider.transform.CompareTag("RunnableWall"))
        {
            rb.useGravity = true;
            cam.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
}