using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class StatManager : MonoBehaviourPun
{
    [SerializeField] private CharacterData characterData;
    public CharacterData CharacterData
    {
        get { return characterData; }
    }
    float[] buffList;

    protected virtual void Awake()
    {
        buffList = new float[System.Enum.GetValues(typeof(Buff)).Length];
    }

    protected virtual void Update()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(Buff)).Length; i++)
        {
            buffList[i] -= Time.deltaTime;
        }
    }

    public void AddBuff(Buff addedBuff, float time)
    {
        if (addedBuff != Buff.Stun || !GetBuff(Buff.UnStoppable))
        {
            photonView.RPC(nameof(AddBuffRPC), RpcTarget.All, addedBuff, time);
        }
    }

    [PunRPC]
    void AddBuffRPC(Buff addedBuff, float time)
    {
        if (buffList[(int)addedBuff] < time && time > 0)
        {
            buffList[(int)addedBuff] = time;
        }
    }

    public bool GetBuff(Buff buff)
    {
        return buffList[(int)buff] > 0;
    }

    public virtual float GetStat(PlayerStat stat)
    {
        float value = 1;
        switch (stat)
        {
            case PlayerStat.Hp:
                value = CharacterData.MaxHp;
                break;
            case PlayerStat.Damage:
                value = 1;
                break;
            case PlayerStat.MoveSpeed:
                value = CharacterData.MoveSpeed;
                break;
            case PlayerStat.AttackSpeed:
                value = 1;
                break;
            case PlayerStat.Luck:
                value = 1;
                break;
            case PlayerStat.Mana:
                value = CharacterData.MaxMana;
                break;
        }
        return value;
    }

    
}
