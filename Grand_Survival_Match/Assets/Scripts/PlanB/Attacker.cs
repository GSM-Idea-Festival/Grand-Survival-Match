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
            targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
            if(currentIndicator != null)
            {
                currentIndicator.transform.rotation = targetRotation;
            }
        }
    }

    Vector3 targetPos;
    public Vector3 TargetPos
    {
        private get { return targetPos; }
        set
        {
            targetPos = value;
            targetPos.y = 0;
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
            StartCoroutine(SpawnAttackFrefab(index,targetRotation));
            coolTime[index] = attackDatas[index].CoolTime;
            statManager.AddBuff((int)Buff.Stun, attackDatas[index].StunTime);
            if (attackDatas[index].IndicatorFrefab != null)
            {
                transform.rotation = targetRotation;
            }
            if (attackDatas[index].DashRange != 0)
            {
                GetComponent<Mover>().UseDash(transform.position + targetRotation * Vector3.forward * Mathf.Min(attackDatas[index].DashRange,Vector3.Distance(transform.position,TargetPos)));
            }
            if (attackDatas[index].Heal != 0)
            {
                photonView.RPC(nameof(RequestHeal), RpcTarget.MasterClient, attackDatas[index].Heal);
            }
            if (attackDatas[index].Barrier != 0)
            {
                photonView.RPC(nameof(RequestBarrier), RpcTarget.MasterClient, attackDatas[index].Barrier);
            }
            return true;
        }
        else
        {
            return false;
        }
    }


    IEnumerator SpawnAttackFrefab(int index,Quaternion rotation)
    {
        yield return new WaitForSeconds(attackDatas[index].SpawnDelayTime);
        if (attackDatas[index].AttackFrefab != null)
        {
            GameObject prefab;
            if (!attackDatas[index].IsNotMelee)
            {
                prefab = PhotonNetwork.Instantiate(attackDatas[index].AttackFrefab.name, transform.position, transform.rotation);
            }
            else
            {
                prefab = PhotonNetwork.Instantiate(attackDatas[index].AttackFrefab.name, TargetPos, Quaternion.identity);
            }
            if (attackDatas[index].IndicatorFrefab != null)
            {
                prefab.transform.rotation = rotation;
            }
            if (!statManager.GetBuff(Buff.DamageUp))
            {
                prefab.GetComponent<HitBox>().Damage = attackDatas[index].Damage * statManager.GetStat(PlayerStat.Damage);
            }
            else
            {
                prefab.GetComponent<HitBox>().Damage = attackDatas[index].Damage * statManager.GetStat(PlayerStat.Damage) * 1.5f;
            }
            prefab.GetComponent<HitBox>().AttackerID = gameObject.GetComponent<PhotonView>().ViewID;
            prefab.GetComponent<HitBox>().AttackerName = PhotonNetwork.NickName;
            prefab.GetComponent<HitBox>().destroyTimer = attackDatas[index].DestroyTimer;
            prefab.GetComponent<HitBox>().ActiveTime = attackDatas[index].ActiveTime;
            prefab.GetComponent<HitBox>().ActiveDelayTime = attackDatas[index].ActiveDelayTime;
            prefab.GetComponent<HitBox>().isAreaSkill = attackDatas[index].IsAreaSkill;
            prefab.GetComponent<HitBox>().GiveBuffs = attackDatas[index].GiveToEnemyBuffs;
            prefab.GetComponent<HitBox>().ShareTimerWrap();
            //prefab.GetComponent<HitBox>().buffs = attackDatas[index].GiveToEnemyBuffs;

            foreach (BuffWithTime buff in attackDatas[index].GetBuffs)
            {
                statManager.AddBuff((int)buff.buff,buff.time);
            }

            
        }
    }

    [PunRPC]
    void RequestHeal(float heal)
    {
        GetComponent<Victim>().TakeHeal(heal);
    }

    [PunRPC]
    void RequestBarrier(float barrier)
    {
        GetComponent<Victim>().AddBarrier(barrier);
    }
}
