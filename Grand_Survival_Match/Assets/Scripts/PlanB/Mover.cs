using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    NavMeshAgent agent;
    

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

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRunning && Vector3.Distance(transform.position,targetPos) <= 0.3f)
        {
            agent.isStopped = true;
        }

    }

    public void UseMove(Vector3 targetPos)
    {
        agent.isStopped = false;
        this.targetPos = targetPos;
        agent.SetDestination(targetPos);

        Quaternion rotationToLookAt = Quaternion.LookRotation(targetPos - transform.position);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));

        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }
}
