using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Controller : MonoBehaviour {
    Animator animator;
    
    public float speed;
    Camera mainCamera;
    RaycastHit hit;
    NavMeshAgent agent { get { return GetComponent<NavMeshAgent>(); } set { value = GetComponent<NavMeshAgent>(); } }
    public GameObject pointerEffect;
    Transform pointer;
    void Start () {
        mainCamera = Camera.main;
        agent.speed = speed;
        pointer = Instantiate(pointerEffect, Vector3.zero, Quaternion.identity).transform;
	}
	
	void Update () {

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            pointer.position = hit.point;
            agent.SetDestination(pointer.position);
        }
	}
}
