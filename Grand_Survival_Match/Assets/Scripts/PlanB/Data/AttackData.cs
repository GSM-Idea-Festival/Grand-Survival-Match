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
    private float mana;
    public float Mana
    {
        get { return mana; }
    }


    [SerializeField]
    private float damage;
    public float Damage
    {
        get { return damage; }
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
    private float stunTime;
    public float StunTime
    {
        get { return stunTime; }
    }
}
