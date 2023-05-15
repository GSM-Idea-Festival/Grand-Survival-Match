using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomUIManager : MonoBehaviour
{
    [SerializeField] CharacterHpBar hpSlider;
    [SerializeField] CoolTimeShower[] skillCoolTime;
    //[SerializeField] Text atkText;
    //[SerializeField] Text defText;
    //[SerializeField] Text dexText;

    public void SetHpUIValue(float value, float shiled, float maxValue)
    {
        hpSlider.SetUIValue(value, shiled, maxValue);
    }

    public void SetCoolTime(int index,float coolTime,float maxCoolTime)
    {
        skillCoolTime[index].SetUIValue(coolTime, maxCoolTime);
    }

    //public void SetStatText(float atk,float def,float dex)
    //{
    //    atkText.text = "ATK : " + (int)atk;
    //    defText.text = "DEF : " + (int)def;
    //    dexText.text = "DEX : " + (int)dex;
    //}
}
