using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Photon.Pun;

public class NicknameLengthCheck : MonoBehaviour
{
    //�г��ӱ��̰� 0�̸� �������� ���ϰ� ����
    public void CheckNameLength(string name)
    {
        gameObject.GetComponent<Selectable>().interactable = name.Length > 0;
    }
}
