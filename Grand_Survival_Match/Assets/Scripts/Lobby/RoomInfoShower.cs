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

    //LobbyManager에서 호출하는 UI 설정용함수
    public void ShowRoomInfo(RoomInfo info)
    {
        roomName = info.Name;
        roomNameText.text = info.Name;
        roomPeopleText.text = info.PlayerCount.ToString() + "/" + info.MaxPlayers.ToString();
    }

    //버튼연결용 wrap
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}
