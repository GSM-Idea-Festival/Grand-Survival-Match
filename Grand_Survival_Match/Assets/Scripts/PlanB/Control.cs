using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Control : MonoBehaviour
{
    PhotonView PV;

    Mover mover;
    Attacker attacker;
    protected Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        mover = GetComponent<Mover>();
        attacker = GetComponent<Attacker>();
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    Move(hit.point);
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
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
