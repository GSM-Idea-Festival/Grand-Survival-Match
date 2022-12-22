using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum BuffType
{
    Hp,     //체력 회복
    Damaged,    //대미지
    HpDamaged,  //현재체력비례 대미지
    LostHpDamaged,  //잃은체력비례 대미지
    Atk,    //기본공격력 증가
    Def,    //방어력 증가
    CurDef, //현재방어력비례 방어력증가
    Asp,    //공격속도 증가 / 감소
    Speed,  //이동속도 증가 / 감소
    Barrier,  //보호막 추가
    HpBarrier,  //최대체력비례 보호막 추가
    Stun,    //스턴
    Unstoppable  //저지불가
}
    
[Serializable]
public struct BuffData
{
    public BuffType buffType;
    public float value;
    public float time;
}
