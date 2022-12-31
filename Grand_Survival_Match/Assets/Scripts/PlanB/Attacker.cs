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
    float[] coolTime;


    GameObject currentIndicator;

    Quaternion targetRotation;
    public Quaternion TargetRotation
    {
        private get { return targetRotation; }
        set
        {
            targetRotation = value;
            if(currentIndicator != null)
            {
                currentIndicator.transform.rotation = value;
            }
        }
    }

    int indicatorIndex = -1;
    public int IndicatorIndex
    {
        get
        {
            return indicatorIndex;
        }
        set
        {
            if (indicatorIndex != value)
            {
                if (currentIndicator != null)
                {
                    Destroy(currentIndicator);
                }
                indicatorIndex = value;
                if (value != -1 && attackDatas[value].IndicatorFrefab != null)
                {
                    currentIndicator = Instantiate(attackDatas[value].IndicatorFrefab, transform);
                    currentIndicator.transform.rotation = TargetRotation;
                }
            }
        }
    }

    void Start()
    {
        statManager = GetComponent<StatManager>();
        attackDatas = statManager.CharacterData.AttackData;
        coolTime = new float[attackDatas.Length];
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        for (int i = 0; i < coolTime.Length; i++)
        {
            coolTime[i] -= Time.deltaTime;
            FindObjectOfType<BottomUIManager>().SetCoolTime(i, coolTime[i], statManager.CharacterData.AttackData[i].CoolTime);
        }
    }


    public bool UseAttack(int index)
    {
        IndicatorIndex = -1;
        if (statManager.GetBuff(Buff.Stun))
        {
            return false;
        }
        if (coolTime[index] <= 0)
        {
            StartCoroutine(SpawnAttackFrefab(index));
            coolTime[index] = attackDatas[index].CoolTime;
            statManager.AddBuff(Buff.Stun, attackDatas[index].StunTime);
            return true;
        }
        else
        {
            return false;
        }
    }


    IEnumerator SpawnAttackFrefab(int index)
    {
        yield return new WaitForSeconds(attackDatas[index].SpawnDelayTime);
        if (attackDatas[index].AttackFrefab != null)
        {
            GameObject prefab = PhotonNetwork.Instantiate(attackDatas[index].AttackFrefab.name, transform.position, transform.rotation);
            if (attackDatas[index].IndicatorFrefab != null)
            {
                prefab.transform.rotation = targetRotation;
            }
            if (!statManager.GetBuff(Buff.DamageUp))
            {
                prefab.GetComponent<HitBox>().Damage = attackDatas[index].Damage * statManager.GetStat(PlayerStat.Damage);
            }
            else
            {
                prefab.GetComponent<HitBox>().Damage = attackDatas[index].Damage * statManager.GetStat(PlayerStat.Damage) * 1.5f;
            }
            prefab.GetComponent<HitBox>().Attacker = gameObject.GetComponent<PhotonView>().ViewID;
            prefab.GetComponent<HitBox>().destroyTimer = attackDatas[index].DestroyTimer;
            prefab.GetComponent<HitBox>().ActiveTime = attackDatas[index].ActiveTime;
            prefab.GetComponent<HitBox>().ActiveDelayTime = attackDatas[index].ActiveDelayTime;
            prefab.GetComponent<HitBox>().ShareTimerWrap();
        }
    }
}
