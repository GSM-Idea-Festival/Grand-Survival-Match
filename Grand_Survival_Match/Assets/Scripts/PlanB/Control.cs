using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Control : MonoBehaviourPun
{

    Mover mover;
    Attacker attacker;
    protected Animator animator;

    KeyCode Qkey = KeyCode.Q;
    KeyCode Wkey = KeyCode.W;
    KeyCode Ekey = KeyCode.E;
    KeyCode Rkey = KeyCode.R;
    KeyCode Tkey = KeyCode.T;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        mover = GetComponent<Mover>();
        attacker = GetComponent<Attacker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);

            attacker.TargetRotation = Quaternion.LookRotation(hit.point - transform.position);

            if (Input.GetMouseButtonDown(1))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    Move(hit.point);
                }
            }

            if (Input.GetKeyDown(Qkey))
            {
                attacker.IndicatorIndex = 0;
            }else if (Input.GetKeyUp(Qkey) && attacker.IndicatorIndex == 0)
            {
                attacker.IndicatorIndex = -1;
                UseAttack(0);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                UseAttack(1);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                UseAttack(2);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                UseAttack(3);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                UseAttack(4);
            }

            if (animator != null)
            {
                if (mover.IsRunning)
                {
                    animator.SetFloat("speed", 1);
                }
                else
                {
                    animator.SetFloat("speed", 0);
                }
            }
        }
    }

    protected virtual void Move(Vector3 targetPos)
    {
        if (mover != null)
        {
            mover.UseMove(targetPos);
        }
    }

    protected virtual void UseAttack(int index)
    {
        if (attacker != null)
        {
            if (attacker.UseAttack(index))
            {
                if (animator != null)
                {
                    animator.SetInteger("skillIndex", index);
                    animator.SetTrigger("useSkill");
                }
            }
        }
    }
}
