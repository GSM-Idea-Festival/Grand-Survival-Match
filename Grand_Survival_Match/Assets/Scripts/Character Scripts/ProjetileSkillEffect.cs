using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjetileSkillEffect : MonoBehaviour
{
    public GameObject Effect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Instantiate(Effect, gameObject.transform.position, gameObject.transform.rotation).transform.parent = gameObject.transform;
    }
}
