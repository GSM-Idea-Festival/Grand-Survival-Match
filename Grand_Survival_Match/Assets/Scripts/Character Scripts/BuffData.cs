using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum BuffType
{
    Hp,     //ü�� ȸ��
    Damaged,    //�����
    HpDamaged,  //����ü�º�� �����
    LostHpDamaged,  //����ü�º�� �����
    Atk,    //�⺻���ݷ� ����
    Def,    //���� ����
    CurDef, //������º�� ��������
    Asp,    //���ݼӵ� ���� / ����
    Speed,  //�̵��ӵ� ���� / ����
    Barrier,  //��ȣ�� �߰�
    HpBarrier,  //�ִ�ü�º�� ��ȣ�� �߰�
    Stun,    //����
    Unstoppable  //�����Ұ�
}
    
[Serializable]
public struct BuffData
{
    public BuffType buffType;
    public float value;
    public float time;
}
