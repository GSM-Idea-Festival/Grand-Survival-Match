using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Data", menuName = "ScriptableObject / Skill Data", order = int.MaxValue)]

public class CharacterSkill : ScriptableObject
{
    [SerializeField]
    public DebuffData[] DebuffDatas;
}
