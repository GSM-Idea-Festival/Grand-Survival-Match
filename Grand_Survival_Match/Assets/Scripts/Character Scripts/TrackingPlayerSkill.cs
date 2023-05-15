using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingPlayerSkill : MonoBehaviour
{
    public GameObject trackingTarget;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(trackingTarget != null)
        {
            transform.position = trackingTarget.transform.position;
        }
    }
}
