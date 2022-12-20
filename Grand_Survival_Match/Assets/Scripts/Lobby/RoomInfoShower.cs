using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfoShower : MonoBehaviour
{
    public Text roomNameText;
    public Text roomPeopleText;
    string roomName;

    public void ShowRoomInfo(RoomInfo info)
    {
        roomName = info.Name;
        roomNameText.text = info.Name;
        roomPeopleText.text = info.PlayerCount.ToString() + "/" + info.MaxPlayers.ToString();
    }

    public void JoinRoom()
    {
        //if (!PhotonNetwork.RejoinRoom(roomName))
        //{
            PhotonNetwork.JoinRoom(roomName);
       // }
    }
}
