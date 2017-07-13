﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TheCamera : MonoBehaviour {

    public float XBorderMin, XBorderMax, YBorderMin, YBorderMax;
    Transform cam { get { return GetComponent<Camera>().transform; } set { value = GetComponent<Camera>().transform; } }
    Transform smTarget;
    public float speed, sensivity, moveDelta;
    Controller controller;
    Vector3 oldPos;

    void Start () {
        smTarget = (new GameObject("smTarget")).transform;
        smTarget.position = cam.position;
        controller = FindObjectOfType<Controller>();
	}

    void Update() 
    {
            float pointer_x = Input.GetAxis("Mouse X");
            float pointer_y = Input.GetAxis("Mouse Y");
            if (Input.touchCount > 0)
            {
                pointer_x = Input.touches[0].deltaPosition.x / 15;
                pointer_y = Input.touches[0].deltaPosition.y / 15;
            }

           
            if (Input.GetMouseButton(0))
            {
                Vector2 currecntPos = Input.mousePosition;         
                smTarget.position -= new Vector3(pointer_x, 0, pointer_y) * sensivity * Time.deltaTime;
                if (Mathf.Abs(Vector3.Distance(oldPos, transform.position)) > moveDelta)
                    controller.nonClickByCam = true;
            }
            else
            {
                controller.nonClickByCam = false;
                oldPos = transform.position;
            } 

            cam.position = Vector3.Lerp(cam.position, smTarget.position, speed * Time.deltaTime);
    }
}
