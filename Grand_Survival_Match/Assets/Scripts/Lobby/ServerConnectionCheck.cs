using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ServerConnectionCheck : MonoBehaviourPunCallbacks
{
    public override void OnConnectedToMaster()
    {
        GetComponent<Selectable>().interactable = true;
    }
}
