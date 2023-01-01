using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviourPun
{
    [SerializeField] float moveSpeed;
    [SerializeField] float moveDelay;

    float moveDelayTimer;

    float damage;
    public float Damage
    {
        get { return damage; }
        set
        {
            photonView.RPC(nameof(ShareDamage), RpcTarget.All, value);
        }
    }

    public float ActiveDelayTime { protected get; set; }
    public float ActiveTime { protected get; set; }
    public float destroyTimer { protected get; set; }
    public bool isAreaSkill { protected get; set; }

    bool isActive = false;
    List<GameObject> counts = new List<GameObject>();

    float areaDelayTimer;

    BuffWithTime[] giveBuffs;
    public BuffWithTime[] GiveBuffs
    {
        get { return giveBuffs; }
        set
        {
            giveBuffs = value;
            photonView.RPC(nameof(ShareBuffs),RpcTarget.MasterClient, giveBuffs);
        }
    }

    int attackerID;
    public int AttackerID
    {
        protected get
        {
            return attackerID;
        }
        set
        {
            attackerID = value;
            photonView.RPC(nameof(ShareAttackerID), RpcTarget.MasterClient, attackerID);
        }
    }

    string attackerName;
    public string AttackerName
    {
        protected get { return attackerName; }
        set
        {
            attackerName = value;
            photonView.RPC(nameof(ShareAttackerName), RpcTarget.MasterClient, attackerName);
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
        moveDelayTimer = moveDelay;
    }

    protected virtual void Update()
    {
        moveDelay -= Time.deltaTime;
        if(PhotonNetwork.IsMasterClient && moveSpeed != 0 && moveDelay <= 0)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }
        areaDelayTimer -= Time.deltaTime;
    }


    protected virtual void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.GetComponent<PhotonView>() != null)
        {
            if (isActive && collision.gameObject.GetComponent<PhotonView>().ViewID != AttackerID && collision.gameObject.GetComponent<Victim>() != null && PhotonNetwork.IsMasterClient)
            {
                if ((isAreaSkill && areaDelayTimer <= 0) || counts.IndexOf(collision.gameObject) == -1) {
                    areaDelayTimer = 0.25f;
                    counts.Add(collision.gameObject);
                    /*if (collision.gameObject.GetComponent<Victim>().TakeDamage(Damage))
                    {
                        FindObjectOfType<GameManager>().Kill();
                    }*/
                    float a = 1;
                    if (isAreaSkill)
                    {
                        a = 0.25f;
                    }

                    if (collision.gameObject.GetComponent<Victim>().TakeDamage(Damage * a))
                    {
                        FindObjectOfType<GameManager>().Kill(attackerName);
                    }
                    foreach (BuffWithTime buff in GiveBuffs)
                    {
                        collision.gameObject.GetComponent<StatManager>().AddBuff((int)buff.buff, buff.time);
                    }
                }
            }
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

    [PunRPC]
    void ShareBuffs(BuffWithTime[] newbuff)
    {
        giveBuffs = newbuff;
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
    void ShareAttackerID(int g)
    {
        attackerID = g;
    }

    [PunRPC]
    void ShareAttackerName(string name)
    {
        attackerName = name;
    }
}
