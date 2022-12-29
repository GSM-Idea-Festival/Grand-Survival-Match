using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class CharacterStats : MonoBehaviourPun
{
    public NavMeshAgent agent;

    protected float maxHP; //최대체력
    protected float hp;    //현재체력
    protected float atk;   //기본공격력
    protected float def;   //방어력
    protected float speed; //이동속도
    protected float barrier; //보호막
    protected float stunTime; //기절시간

    protected float qCooltime;
    protected float wCooltime;
    protected float eCooltime;
    protected float rCooltime;
    protected float tCooltime;

    protected bool isUnstoppable;   //저지불가
    protected bool isSkillUsing;    //스킬사용중
    protected bool isDead
    {
        get
        {
            return false;
        }
        set
        {
            FindObjectOfType<GameManager>().RespawnRequest(gameObject);
        }
    }        //사망

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
    //프로퍼티

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

        if (photonView.IsMine)
        {
            SkillIndicatorActivate();
        }
    }

    [PunRPC]
    public void ApplyUpdateHP(float hp)
    {
        this.hp = hp;
    }

    [PunRPC]
    public void Damaged(float damage)
    {
        if (PhotonNetwork.IsMasterClient)
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
                        hp = 0;
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
                    hp = 0;
                }
                else
                {
                    hp -= damage * (damage / (damage + def));
                }
            }

            photonView.RPC("ApplyUpdateHP", RpcTarget.Others, hp);
        }
    }   //대미지

    [PunRPC]
    public void HpDamaged(float percent)
    {
        if (PhotonNetwork.IsMasterClient)
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
                        hp = 0;
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
                    hp = 0;
                }
                else
                {
                    hp -= damage * (damage / (damage + def));
                }
            }

            photonView.RPC("ApplyUpdateHP", RpcTarget.Others, hp);
        }
    }   //현재체력 비례 대미지

    [PunRPC]
    public void LostHpDamaged(float percent)
    {
        if (PhotonNetwork.IsMasterClient)
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
                        hp = 0;
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
                    hp = 0;
                }
                else
                {
                    hp -= damage * (damage / (damage + def));
                }
            }

            photonView.RPC("ApplyUpdateHP", RpcTarget.Others, hp);
        }
    }   //잃은체력 비례 대미지

    public void Heal(int value)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (hp + value > maxHP)
            {
                hp = maxHP;
            }
            else
            {
                hp += value;
            }

            photonView.RPC("ApplyUpdateHP", RpcTarget.Others, hp);
        }
    }   //체력회복

    #region 스킬사용
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
    }   //스턴 설정

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
    }   //스킬쿨타임 초기화

    protected virtual void SkillIndicatorActivate()
    {

    }

    public IEnumerator SetATK(float value, float time)
    {
        atk += value;
        yield return new WaitForSeconds(time);
        atk -= value;
        yield break;
    }   //기본공격력 설정

    public IEnumerator SetDEF(float value, float time)
    {
        def += value;
        yield return new WaitForSeconds(time);
        def -= value;
        yield break;
    }   //방어력 설정
    
    public IEnumerator SetPercentDEF(float value, float time)
    {
        float addDef = def * value;
        def += addDef;
        yield return new WaitForSeconds(time);
        def -= addDef;
        yield break;
    }   //현재 방어력 비례 방어력 증가

    public IEnumerator SetHpDef(float value, float time)
    {
        float addDef = maxHP * value;
        def += addDef;
        yield return new WaitForSeconds(time);
        def -= addDef;
        yield break;
    }   //최대 체력 비례 방어력 증가

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
    }   //이동속도 설정

    public IEnumerator SetBarrier(float value, float time)
    {
        barrier += value;
        yield return new WaitForSeconds(time);
        barrier -= value;
        yield break;
    }   //보호막 설정

    public IEnumerator SetHpBarrier(float value, float time)
    {
        float addBarrier = maxHP * value;
        barrier += addBarrier;
        yield return new WaitForSeconds(time);
        barrier -= addBarrier;
        yield break;
    }   //최대체력비례 보호막 설정

    public IEnumerator SetUnstoppable(float time)
    {
        isUnstoppable = true;
        yield return new WaitForSeconds(time);
        isUnstoppable = false;
    }   //저지불가 설정
}
