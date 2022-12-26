using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum DebuffType
{
    Damaged,    //�����
    HpDamaged,  //����ü�º�� �����
    LostHpDamaged,  //����ü�º�� �����
    Speed,  //�̵��ӵ� ����
    Stun,    //����
}

[Serializable]
public struct DebuffData
{
    public DebuffType debuffType;
    public float value;
    public float time;
}