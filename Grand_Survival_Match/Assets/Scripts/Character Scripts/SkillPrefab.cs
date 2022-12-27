using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPrefab : MonoBehaviour
{
    public GameObject Attacker;

    public GameObject[] Target;

    public CharacterSkill CharacterSkill;

    public int count = 0;

    public float time = 0.1f;

    public bool isSpearMan = false;
    public bool isWizardQSkill = false;

    private void Start()
    {
        Destroy(gameObject, time);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != Attacker && other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < CharacterSkill.DebuffDatas.Length; i++)
            {
                switch (CharacterSkill.DebuffDatas[i].debuffType)
                {
                    case DebuffType.Damaged:
                        other.GetComponent<CharacterStats>().Damaged(CharacterSkill.DebuffDatas[i].value);
                        if (isSpearMan)
                        {
                            Attacker.GetComponent<SpearMan>().Heal(10);
                        }
                        if (isWizardQSkill)
                        {
                            Attacker.GetComponent<Wizard>().SkillCoolInit(1);
                        }
                        count++;
                        break;
                    case DebuffType.HpDamaged:
                        other.GetComponent<CharacterStats>().HpDamaged(CharacterSkill.DebuffDatas[i].value);
                        if (isSpearMan)
                        {
                            Attacker.GetComponent<SpearMan>().Heal(10);
                        }
                        count++;
                        break;
                    case DebuffType.LostHpDamaged:
                        other.GetComponent<CharacterStats>().LostHpDamaged(CharacterSkill.DebuffDatas[i].value);
                        if (isSpearMan)
                        {
                            Attacker.GetComponent<SpearMan>().Heal(10);
                        }
                        count++;
                        break;
                    case DebuffType.Speed:
                        other.GetComponent<CharacterStats>().SetSpeed(CharacterSkill.DebuffDatas[i].value, CharacterSkill.DebuffDatas[i].time);
                        count++;
                        break;
                    case DebuffType.Stun:
                        other.GetComponent<CharacterStats>().SetStun(CharacterSkill.DebuffDatas[i].time);
                        count++;
                        break;
                }
            }
        }
    }
}
