using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    NavMeshAgent agent;
    StatManager statManager;

    public bool IsRunning
    {
        get
        {
            return !agent.isStopped;
        }
    }
    float rotateSpeedMovement = 0.1f;
    float rotateVelocity;

    Vector3 targetPos;
    Vector3 dashTargetPos;
    bool isDashing;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        statManager = GetComponent<StatManager>();
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRunning && Vector3.Distance(transform.position,targetPos) <= 0.3f)
        {
            agent.isStopped = true;
        }
        if (statManager.GetBuff(Buff.Stun))
        {
            agent.isStopped = true;
        }

        if (isDashing)
        {
            if(Vector3.Distance(transform.position,dashTargetPos) <= 0.3)
            {
                isDashing = false;
            }
            else
            {
                agent.isStopped = true;
                transform.position = Vector3.Lerp(transform.position, dashTargetPos,Time.deltaTime * 6);
            }
        }
    }

    public void UseMove(Vector3 targetPos)
    {
        this.targetPos = targetPos;
        if (!statManager.GetBuff(Buff.Stun))
        {
            agent.isStopped = false;
            agent.SetDestination(targetPos);

            Quaternion rotationToLookAt = Quaternion.LookRotation(targetPos - transform.position);
            float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));

            transform.eulerAngles = new Vector3(0, rotationY, 0);
        }
    }

    public void UseDash(Vector3 targetPos)
    {
        isDashing = true;
        agent.isStopped = true;
        dashTargetPos = targetPos;
    }
}
