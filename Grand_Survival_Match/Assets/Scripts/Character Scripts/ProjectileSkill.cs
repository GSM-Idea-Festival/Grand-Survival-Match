using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : MonoBehaviour
{
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, speed);
    }
}
