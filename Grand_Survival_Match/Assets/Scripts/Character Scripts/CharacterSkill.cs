using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Data", menuName = "ScriptableObject / Skill Data", order = int.MinValue)]

public class CharacterSkill : ScriptableObject
{
    [SerializeField]
    private GameObject SkillPrefabs;

    [SerializeField]
    BuffData[] MyBuffData;

    [SerializeField]
    BuffData[] EnemyBuffData;
}
