using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Object/Character Data", order = int.MaxValue)]

public class CharacterData : ScriptableObject
{
    [SerializeField]
    private string characterName;
    public string CharacterName
    {
        get { return characterName; }
    }


    [SerializeField]
    private float maxHp;
    public float MaxHp
    {
        get { return maxHp; }
    }


    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
    }


    [SerializeField]
    private AttackData[] attackData;
    public AttackData[] AttackData
    {
        get { return attackData; }
    }


    [SerializeField]
    private float maxMana;
    public float MaxMana
    {
        get { return maxMana; }
    }
}
