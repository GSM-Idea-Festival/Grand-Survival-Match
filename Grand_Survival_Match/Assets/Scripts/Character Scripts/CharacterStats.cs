using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterStats : MonoBehaviour
{
    public NavMeshAgent agent;

    protected float maxHP; //�ִ�ü��
    protected float hp;    //����ü��
    protected float atk;   //�⺻���ݷ�
    protected float def;   //����
    protected float speed; //�̵��ӵ�
    protected float barrier; //��ȣ��
    protected float stunTime; //�����ð�

    protected float qCooltime;
    protected float wCooltime;
    protected float eCooltime;
    protected float rCooltime;
    protected float tCooltime;

    protected bool isUnstoppable;   //�����Ұ�
    protected bool isSkillUsing;    //��ų�����
    protected bool isDead;          //���

    #region Property
    public float MaxHP { get { return maxHP; } }
    public float Hp { get {  return hp;} }
    public float Atk { get { return atk;} }
    public float Def { get { return def; } }
    public float Speed { get { return speed; } }
    public float Barrier { get { return barrier;} }
    public float StunTime { get { return stunTime;} }
    public float QCooltime { get { return qCooltime;} }
    public float WCooltime { get { return wCooltime;} }
    public float ECooltime { get { return eCooltime;} }
    public float RCooltime { get { return rCooltime;} }
    public float TCooltime { get { return tCooltime;} }
    public bool IsUnstoppable { get { return isUnstoppable; } }
    public bool IsSkillUsing { get { return isSkillUsing; } }
    public bool IsDead { get { return isDead; } }
    #endregion
    //������Ƽ

    private void Start()
    {
    }

    protected virtual void Update()
    {
        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
        }

        if (qCooltime > 0)
        {
            qCooltime -= Time.deltaTime;
        }

        if (wCooltime > 0)
        {
            wCooltime -= Time.deltaTime;
        }

        if (eCooltime > 0)
        {
            eCooltime -= Time.deltaTime;
        }

        if (rCooltime > 0)
        {
            rCooltime -= Time.deltaTime;
        }

        if (tCooltime > 0)
        {
            tCooltime -= Time.deltaTime;
        }

        agent.speed = speed;
    }

    public void Damaged(float damage)
    {
        if (barrier > 0)
        {
            if (barrier >= damage)
            {
                barrier -= damage;
            }
            else
            {
                damage -= barrier;
                barrier = 0;
                if (hp - damage * (damage / (damage + def)) <= 0)
                {
                    isDead = true;
                }
                else
                {
                    hp -= damage * (damage / (damage + def));
                }
            }
        }
        else
        {
            if (hp - damage * (damage / (damage + def)) <= 0)
            {
                isDead=true;
            }
            else
            {
                hp -= damage * (damage / (damage + def));
            }
        }
        
    }   //�����

    public void HpDamaged(float percent)
    {
        float damage = hp * percent;
        if (barrier > 0)
        {
            if (barrier >= damage)
            {
                barrier -= damage;
            }
            else
            {
                damage -= barrier;
                barrier = 0;
                if (hp - damage * (damage / (damage + def)) <= 0)
                {
                    isDead = true;
                }
                else
                {
                    hp -= damage * (damage / (damage + def));
                }
            }
        }
        else
        {
            if (hp - damage * (damage / (damage + def)) <= 0)
            {
                isDead = true;
            }
            else
            {
                hp -= damage * (damage / (damage + def));
            }
        }
    }   //����ü�� ��� �����

    public void LostHpDamaged(float percent)
    {
        float damage = (maxHP - hp) * percent;
        if (barrier > 0)
        {
            if (barrier >= damage)
            {
                barrier -= damage;
            }
            else
            {
                damage -= barrier;
                barrier = 0;
                if (hp - damage * (damage / (damage + def)) <= 0)
                {
                    isDead = true;
                }
                else
                {
                    hp -= damage * (damage / (damage + def));
                }
            }
        }
        else
        {
            if (hp - damage * (damage / (damage + def)) <= 0)
            {
                isDead = true;
            }
            else
            {
                hp -= damage * (damage / (damage + def));
            }
        }
    }   //����ü�� ��� �����

    public void Heal(int value)
    {
        if (hp + value > maxHP)
        {
            hp = maxHP;
        }
        else
        {
            hp += value;
        }
    }   //ü��ȸ��

    #region ��ų���
    protected virtual void UseQ(float coolTime)
    {
        qCooltime = coolTime;
    }
    protected virtual void UseW(float coolTime)
    {
        wCooltime = coolTime;
    }
    protected virtual void UseE(float coolTime)
    {
        eCooltime = coolTime;
    }
    protected virtual void UseR(float coolTime)
    {
        rCooltime = coolTime;
    }
    protected virtual void UseT(float coolTime)
    {
        tCooltime = coolTime;
    }
    #endregion

    public void SetStun(float time)
    {
        if (isUnstoppable)
        {
            return;
        }
        stunTime = time;
    }   //���� ����

    public void SkillCoolInit(int skillIndex)
    {
        switch (skillIndex)
        {
            case 0:
                qCooltime = 0;
                break;
            case 1:
                wCooltime = 0;
                break;
            case 2:
                eCooltime = 0;
                break;
            case 3:
                rCooltime = 0;
                break;
            case 4:
                tCooltime = 0;
                break;
        }
    }   //��ų��Ÿ�� �ʱ�ȭ

    public IEnumerator SetATK(float value, float time)
    {
        atk += value;
        yield return new WaitForSeconds(time);
        atk -= value;
        yield break;
    }   //�⺻���ݷ� ����

    public IEnumerator SetDEF(float value, float time)
    {
        def += value;
        yield return new WaitForSeconds(time);
        def -= value;
        yield break;
    }   //���� ����
    
    public IEnumerator SetPercentDEF(float value, float time)
    {
        float addDef = def * value;
        def += addDef;
        yield return new WaitForSeconds(time);
        def -= addDef;
        yield break;
    }   //���� ���� ��� ���� ����

    public IEnumerator SetHpDef(float value, float time)
    {
        float addDef = maxHP * value;
        def += addDef;
        yield return new WaitForSeconds(time);
        def -= addDef;
        yield break;
    }   //�ִ� ü�� ��� ���� ����

    public IEnumerator SetSpeed(float value, float time)
    {
        if (value < 0)
        {
            if (isUnstoppable) yield break;
        }
        speed += value;
        yield return new WaitForSeconds(time);
        speed -= value;
        yield break;
    }   //�̵��ӵ� ����

    public IEnumerator SetBarrier(float value, float time)
    {
        barrier += value;
        yield return new WaitForSeconds(time);
        barrier -= value;
        yield break;
    }   //��ȣ�� ����

    public IEnumerator SetHpBarrier(float value, float time)
    {
        float addBarrier = maxHP * value;
        barrier += addBarrier;
        yield return new WaitForSeconds(time);
        barrier -= addBarrier;
        yield break;
    }   //�ִ�ü�º�� ��ȣ�� ����

    public IEnumerator SetUnstoppable(float time)
    {
        isUnstoppable = true;
        yield return new WaitForSeconds(time);
        isUnstoppable = false;
    }   //�����Ұ� ����
}
