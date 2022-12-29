using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class Attacker : MonoBehaviourPun
{
    StatManager statManager;
    AttackData[] attackDatas;
    public float Mana { get; private set; }
    float[] coolTime;

    void Start()
    {
        statManager = GetComponent<StatManager>();
        attackDatas = statManager.CharacterData.AttackData;
        coolTime = new float[attackDatas.Length];
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Mana = Mathf.Min(Mana + Time.deltaTime, statManager.GetStat(PlayerStat.Mana));
        for (int i = 0; i < coolTime.Length; i++)
        {
            coolTime[i] -= Time.deltaTime;
        }
    }

    public bool UseAttack(int index)
    {
        
        if (statManager.GetBuff(Buff.Stun))
        {
            return false;
        }
        if (coolTime[index] <= 0)
        {
            UseAttackRPC(index);
            return true;
        }
        else
        {
            return false;
        }
    }

    void UseAttackRPC(int index)
    {
        StartCoroutine(SpawnAttackFrefab(index));
        coolTime[index] = attackDatas[index].CoolTime;
        statManager.AddBuff(Buff.Stun, attackDatas[index].StunTime);
    }

    IEnumerator SpawnAttackFrefab(int index)
    {
        yield return new WaitForSeconds(attackDatas[index].SpawnDelayTime);
        if (attackDatas[index].AttackFrefab != null)
        {
            GameObject prefab = PhotonNetwork.Instantiate(attackDatas[index].AttackFrefab.name, transform.position, transform.rotation);
            if (!statManager.GetBuff(Buff.DamageUp))
            {
                prefab.GetComponent<HitBox>().Damage = attackDatas[index].Damage * statManager.GetStat(PlayerStat.Damage);
            }
            else
            {
                prefab.GetComponent<HitBox>().Damage = attackDatas[index].Damage * statManager.GetStat(PlayerStat.Damage) * 1.5f;
            }
            prefab.GetComponent<HitBox>().attacker = gameObject;
            
        }
    }
}
