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
                hp = Mathf.Clamp(0, value, statManager.GetStat(PlayerStat.Hp));
                photonView.RPC(nameof(ShareHP), RpcTarget.Others,hp);
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
    }

    [PunRPC]
    void SpawnHpBar(string name)
    {
        
        FindObjectOfType<GameManager>().spawnHpBar(gameObject, name);
    }

    [PunRPC]
    public void TakeDamage(float damage)
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
                //return true;
            }
            
        }

        //return false;
    }

    [PunRPC]
    void ShareHP(float newHp)
    {
        hp = newHp;
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
