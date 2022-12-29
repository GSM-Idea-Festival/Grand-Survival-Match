using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkillPrefab : MonoBehaviourPun
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
        if (other.gameObject != Attacker && other.GetComponent<CharacterStats>() != null)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            for (int i = 0; i < CharacterSkill.DebuffDatas.Length; i++)
            {
                switch (CharacterSkill.DebuffDatas[i].debuffType)
                {
                    case DebuffType.Damaged:
                        other.GetComponent<CharacterStats>().photonView.RPC("Damaged", RpcTarget.All, CharacterSkill.DebuffDatas[i].value);
                        if (isSpearMan)
                        {
                            Attacker.GetComponent<SpearMan>().photonView.RPC("Heal", RpcTarget.All, 10);
                        }
                        if (isWizardQSkill)
                        {
                            Attacker.GetComponent<Wizard>().SkillCoolInit(1);
                        }
                        count++;
                        break;
                    case DebuffType.HpDamaged:
                        other.GetComponent<CharacterStats>().photonView.RPC("HpDamaged", RpcTarget.All, CharacterSkill.DebuffDatas[i].value);
                        if (isSpearMan)
                        {
                            Attacker.GetComponent<SpearMan>().Heal(10);
                        }
                        count++;
                        break;
                    case DebuffType.LostHpDamaged:
                        other.GetComponent<CharacterStats>().photonView.RPC("LostHpDamaged", RpcTarget.All, CharacterSkill.DebuffDatas[i].value);
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
