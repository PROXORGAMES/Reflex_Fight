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
    [HideInInspector]
    public bool nonClick;
    int skill = -1;

    void Start () {
        mainCamera = Camera.main;
        agent.speed = speed;
        pointer = Instantiate(pointerEffect, Vector3.zero, Quaternion.identity).transform;
	}
	
	void Update () {
        agent.speed = speed;
        if (!nonClick)
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                pointer.position = hit.point;
                if (Input.GetMouseButtonDown(0))
                    switch (skill)
                    {
                        case -1:
                             agent.SetDestination(pointer.position);
                            break;
                        case 0:
                            TeleportSkill();
                            break;
                    }
            }
        }
	}

    public void SetSkill(int num)
    {
        switch(num)
        {
            case 0:
                skill = 0;
                break;
        }
    }

    void TeleportSkill()
    {
        agent.SetDestination(pointer.position);
        transform.position = new Vector3(pointer.position.x, transform.position.y, pointer.position.z);
        skill = -1;
    }
}
