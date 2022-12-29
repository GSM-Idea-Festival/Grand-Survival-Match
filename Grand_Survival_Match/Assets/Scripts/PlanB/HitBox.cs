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

    public float activeDelayTime;
    public float activeTime;
    public float destroyTimer;

    bool isActive = false;

    public GameObject attacker { protected get; set; }

    protected virtual void Start()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(DestroyTimer());
        }
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(ActiveDelayTimer());
        }
    }


    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (isActive && collision.gameObject != attacker && collision.gameObject.GetComponent<Victim>() != null && PhotonNetwork.IsMasterClient)
        {
            collision.gameObject.GetComponent<Victim>().TakeDamage(Damage);
        }
    }

    public void ShareTimerWrap()
    {
        photonView.RPC(nameof(ShareTimers), RpcTarget.MasterClient, activeDelayTime, activeTime, destroyTimer);
    }

    [PunRPC]
    void ShareTimers(float activeDelayTime, float activeTime, float destroyTimer)
    {
        this.activeDelayTime = activeDelayTime;
        this.activeTime = activeTime;
        this.destroyTimer = destroyTimer;
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(destroyTimer);
        PhotonNetwork.Destroy(gameObject);
    }

    IEnumerator ActiveDelayTimer()
    {
        yield return new WaitForSeconds(activeDelayTime);
        isActive = true;
        StartCoroutine(UnActiveTimer());
    }

    IEnumerator UnActiveTimer()
    {
        yield return new WaitForSeconds(activeTime);
        isActive = false;
    }

    [PunRPC]
    void ShareDamage(float newdamage)
    {
        damage = newdamage;
    }
}
