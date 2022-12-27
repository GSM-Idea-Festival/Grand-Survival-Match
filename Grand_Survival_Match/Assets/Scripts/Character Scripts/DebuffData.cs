using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum DebuffType
{
    Damaged,    //대미지
    HpDamaged,  //현재체력비례 대미지
    LostHpDamaged,  //잃은체력비례 대미지
    Speed,  //이동속도 감소
    Stun,    //스턴
}

[Serializable]
public struct DebuffData
{
    public DebuffType debuffType;
    public float value;
    public float time;
}