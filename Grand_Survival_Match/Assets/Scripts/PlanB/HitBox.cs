using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviourPun
{
    float damage;
    public float Damage
    {
        get { return damage; }
        set {
            photonView.RPC(nameof(ShareDamage), RpcTarget.All, value);
        }
    }
    public GameObject attacker { protected get; set; }

    protected virtual void Start()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(Timer(0.1f));
        }
    }


    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject != attacker && collision.gameObject.GetComponent<Victim>() != null && PhotonNetwork.IsMasterClient)
        {
            collision.gameObject.GetComponent<Victim>().TakeDamage(Damage);
        }
    }

    IEnumerator Timer(float timer)
    {
        yield return new WaitForSeconds(timer);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    void ShareDamage(float newdamage)
    {
        Debug.Log(newdamage);
        damage = newdamage;
    }
}
