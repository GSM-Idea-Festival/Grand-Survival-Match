using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Data", menuName = "Scriptable Object/Attack Data", order = int.MinValue)]
public class AttackData : ScriptableObject
{
    [SerializeField]
    private GameObject attackFrefab;
    public GameObject AttackFrefab
    {
        get { return attackFrefab; }
    }

    [SerializeField]
    private GameObject indicatorFrefab;
    public GameObject IndicatorFrefab
    {
        get { return indicatorFrefab; }
    }


    [SerializeField]
    private float damage;
    public float Damage
    {
        get { return damage; }
    }


    [SerializeField]
    private float heal;
    public float Heal
    {
        get { return heal; }
    }


    [SerializeField]
    private float barrier;
    public float Barrier
    {
        get { return barrier; }
    }


    [SerializeField]
    private BuffWithTime[] giveToEnemyBuffs;
    public BuffWithTime[] GiveToEnemyBuffs
    {
        get { return giveToEnemyBuffs; }
    }


    [SerializeField]
    private BuffWithTime[] getBuffs;
    public BuffWithTime[] GetBuffs
    {
        get { return getBuffs; }
    }


    [SerializeField]
    private float coolTime;
    public float CoolTime
    {
        get { return coolTime; }
    }

    [SerializeField]
    private float spawnDelayTime;
    public float SpawnDelayTime
    {
        get { return spawnDelayTime; }
    }

    [SerializeField]
    private float activeDelayTime;
    public float ActiveDelayTime
    {
        get { return activeDelayTime; }
    }

    [SerializeField]
    private float activeTime;
    public float ActiveTime
    {
        get { return activeTime; }
    }

    [SerializeField]
    private float destroyTimer;
    public float DestroyTimer
    {
        get { return destroyTimer; }
    }

    [SerializeField]
    private float stunTime;
    public float StunTime
    {
        get { return stunTime; }
    }


    [SerializeField]
    private float dashRange;
    public float DashRange
    {
        get { return dashRange; }
    }


    [SerializeField]
    private bool isNotMelee;
    public bool IsNotMelee
    {
        get { return isNotMelee; }
    }


    [SerializeField]
    private bool isAreaSkill;
    public bool IsAreaSkill
    {
        get { return isAreaSkill; }
    }   

}
