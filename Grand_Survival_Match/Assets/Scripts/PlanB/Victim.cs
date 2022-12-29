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
                Debug.Log(value);
                hp = Mathf.Clamp(0, value, statManager.GetStat(PlayerStat.Hp));
                photonView.RPC(nameof(ShareHP), RpcTarget.Others,hp);
            }
        }
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        statManager = GetComponent<StatManager>();
        Hp = statManager.GetStat(PlayerStat.Hp);
    }



    [PunRPC]
    public void TakeDamage(float damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터클라이언트");
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
            }
            
        }

       
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
    }
}
