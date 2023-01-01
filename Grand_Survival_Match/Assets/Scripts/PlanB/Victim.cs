using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Victim : MonoBehaviourPun
{
    StatManager statManager;


    private float hp;
    public float Hp
    {
        get { return hp; }
        private set
        {
            if (PhotonNetwork.IsMasterClient)
            {
                hp = Mathf.Clamp(value, 0, statManager.GetStat(PlayerStat.Hp));
                photonView.RPC(nameof(ShareHP), RpcTarget.Others, hp);
            }
        }
    }

    private float barrier;
    public float Barrier
    {
        get { return barrier; }
        set
        {
            if (PhotonNetwork.IsMasterClient)
            {
                barrier = Mathf.Max(0, value);
                photonView.RPC(nameof(ShareBarrier), RpcTarget.Others, barrier);
            }
        }
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            photonView.RPC(nameof(SpawnHpBar), RpcTarget.All, PhotonNetwork.NickName);
        }
    }

    private void OnEnable()
    {
        statManager = GetComponent<StatManager>();
        Hp = statManager.GetStat(PlayerStat.Hp);
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            FindObjectOfType<BottomUIManager>().GetComponentInChildren<CharacterHpBar>().SetUIValue(Hp, statManager.GetStat(PlayerStat.Hp), 0);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            if (Barrier > 0)
            {
                Barrier -= Time.deltaTime * 25;
            }
        }
    }

    [PunRPC]
    void SpawnHpBar(string name)
    {

        FindObjectOfType<GameManager>().spawnHpBar(gameObject, name);
    }

    [PunRPC]
    public bool TakeDamage(float damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!statManager.GetBuff(Buff.Immune))
            {
                if (!statManager.GetBuff(Buff.Defence))
                {
                    Hp -= damage;
                }
                else
                {
                    Hp -= damage * 0.5f;
                }
            }

            photonView.RPC(nameof(TakeDamage), RpcTarget.Others, damage);
            if (Hp <= 0)
            {
                photonView.RPC(nameof(RespawnRequest), RpcTarget.All);
                return true;
            }

        }

        return false;
    }

    public void AddBarrier(float barrier)
    {
        this.barrier += barrier;
    }

    public void TakeHeal(float heal)
    {
        Hp += heal;
    }

    [PunRPC]
    void ShareHP(float newHp)
    {
        hp = newHp;
    }

    [PunRPC]
    void ShareBarrier(float newBarrier)
    {
        barrier = newBarrier;
    }

    /*public void TakeHeal(float heal)
    {
        hp += heal;
    }*/

    [PunRPC]
    void RespawnRequest()
    {
        FindObjectOfType<GameManager>().RespawnRequest(gameObject);
        if (photonView.IsMine)
        {
            FindObjectOfType<GameManager>().Death();
        }
    }
}
