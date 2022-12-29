using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Victim : MonoBehaviourPun
{
    StatManager statManager;

    private float hp;
    public float HP
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
        statManager = GetComponent<StatManager>();
        hp = statManager.GetStat(PlayerStat.Hp);
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
                    hp -= damage;
                }
                else
                {
                    hp -= damage * 0.5f;
                }
            }

            photonView.RPC(nameof(TakeDamage), RpcTarget.Others, damage);
            FindObjectOfType<GameManager>().RespawnRequest(gameObject);
        }

        if (hp <= 0)
        {
            
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
}
