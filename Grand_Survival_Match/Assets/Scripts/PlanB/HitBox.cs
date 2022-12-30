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

    public float ActiveDelayTime { protected get; set; }
    public float ActiveTime { protected get; set; }
    public float destroyTimer { protected get; set; }

    bool isActive = false;
    List<GameObject> counts = new List<GameObject>();

    int attacker;
    public int Attacker {
        protected get
        {
            return attacker;
        }
        set
        {
            attacker = value;
            photonView.RPC(nameof(ShareAttacker), RpcTarget.MasterClient, attacker);
        }
    }

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


    protected virtual void OnTriggerStay(Collider collision)
    {
        if (isActive && collision.gameObject.GetComponent<PhotonView>().ViewID != Attacker && collision.gameObject.GetComponent<Victim>() != null && PhotonNetwork.IsMasterClient && counts.IndexOf(collision.gameObject)==-1)
        {
            counts.Add(collision.gameObject);
            Debug.Log("hit");
            /*if (collision.gameObject.GetComponent<Victim>().TakeDamage(Damage))
            {
                FindObjectOfType<GameManager>().Kill();
            }*/
            collision.gameObject.GetComponent<Victim>().TakeDamage(Damage);
        }
    }

    public void ShareTimerWrap()
    {
        photonView.RPC(nameof(ShareTimers), RpcTarget.MasterClient, ActiveDelayTime, ActiveTime, destroyTimer);
    }

    [PunRPC]
    void ShareTimers(float activeDelayTime, float activeTime, float destroyTimer)
    {
        this.ActiveDelayTime = activeDelayTime;
        this.ActiveTime = activeTime;
        this.destroyTimer = destroyTimer;
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(destroyTimer);
        PhotonNetwork.Destroy(gameObject);
    }

    IEnumerator ActiveDelayTimer()
    {
        yield return new WaitForSeconds(ActiveDelayTime);
        isActive = true;
        StartCoroutine(UnActiveTimer());
    }

    IEnumerator UnActiveTimer()
    {
        yield return new WaitForSeconds(ActiveTime);
        isActive = false;
    }

    [PunRPC]
    void ShareDamage(float newdamage)
    {
        damage = newdamage;
    }

    [PunRPC]
    void ShareAttacker(int g)
    {
        attacker = g;
    }
}
