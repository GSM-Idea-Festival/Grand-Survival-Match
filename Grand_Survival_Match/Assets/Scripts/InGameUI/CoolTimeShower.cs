using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeShower : MonoBehaviour
{
    public Image cover;
    public Text text;

    public void SetUIValue(float value,float maxValue)
    {
        text.text = ((int)value).ToString();
        cover.fillAmount = value/maxValue;
        if(value > 0)
        {
            text.gameObject.SetActive(true);
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }
}
