using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Photon.Pun;

public class NicknameLengthCheck : MonoBehaviour
{
    //닉네임길이가 0이면 시작하지 못하게 제한
    public void CheckNameLength(string name)
    {
        gameObject.GetComponent<Selectable>().interactable = name.Length > 0;
    }
}
