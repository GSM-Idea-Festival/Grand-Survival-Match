using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public float damage { protected get; set; }
    GameObject attacker;

    protected virtual void Start()
    {
        StartCoroutine(Timer(0.1f));
    }


    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject != attacker&& collision.gameObject.GetComponent<Victim>() != null)
        {
            collision.gameObject.GetComponent<Victim>().TakeDamage(damage);
        }
    }

    IEnumerator Timer(float timer)
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
