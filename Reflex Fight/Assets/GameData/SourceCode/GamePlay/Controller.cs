using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class Controller : MonoBehaviour {

    public enum typeOfSkill
    {
        directed, non_directed, directed_on_target
    }

    [Serializable]
    public struct Skill
    {
        public typeOfSkill TypeOfSkill;
        public string SkillName, Discription;
        public float Range, UseTime, Cooldown, Param;
        bool onCooldown;
        [HideInInspector]
        public Vector3 pos;
    }

    public Animator animator;
    public GameObject test;
    public float stopAnimDistance;
    Camera mainCamera;
    RaycastHit hit;
    NavMeshAgent agent { get { return GetComponent<NavMeshAgent>(); } set { value = GetComponent<NavMeshAgent>(); } }
    public GameObject pointerEffect;
    Transform pointer;
    [HideInInspector]
    public bool nonClick, nonClickByCam, usedTeleport;
    int skill = -1, animNum;
    float speed;
    public Skill[] skills;

    void Start () {
        mainCamera = Camera.main;
        pointer = Instantiate(pointerEffect, Vector3.zero, Quaternion.identity).transform;
        speed = agent.speed;
	}
	
	void Update () {

        if (!nonClick && !nonClickByCam)
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                pointer.position = hit.point;
                if (Input.GetMouseButtonUp(0))
                {
                    switch (skill)
                    {
                        case -1:
                            animator.CrossFade("Move", 0.1f);
                            agent.SetDestination(pointer.position);
                            break;
                        case 0:
                            Vector3 target;
                            if (Vector3.Distance(transform.position, pointer.position) > skills[skill].Range)
                                target = transform.position + (pointer.position - transform.position).normalized * skills[skill].Range;
                            else
                                target = pointer.position;
                            skills[skill].pos = target + new Vector3(0, agent.height/2, 0);
                            agent.SetDestination(skills[skill].pos);
                            usedTeleport = true;
                            break;
                    }
                }
            }
        }
        Animatate();
        if (usedTeleport)
            TeleportSkill();
    }

    public void SetSkill(int num)
    {
        skill = num;
    }

    void Animatate()
    {
        if(test)
            test.transform.position = agent.pathEndPosition;
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(agent.pathEndPosition.x, agent.pathEndPosition.z)) <= stopAnimDistance)
            animNum = 0;
        else
            animNum = 1;
        animator.SetInteger("Num", animNum);
    }

    public void TeleportSkill()
    {
        agent.enabled = false;
        transform.LookAt(skills[skill].pos);
        if (transform.position != skills[skill].pos)
            transform.position = Vector3.MoveTowards(transform.position, skills[skill].pos, skills[skill].Param);
        else
        {
            skill = -1;
            agent.enabled = true;
            usedTeleport = false;
        }
    }
}
 