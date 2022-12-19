using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    static LobbyManager instance;
    public GameObject roomSelectLayer;
    public GameObject roomLayer;
    public GameObject roomInformationPrefab;
    public GameObject roomListLayer;
    public GameObject playerListLayer;
    public GameObject playerNamePrefab;
    public GameObject playerNameLayer;
    Dictionary<Player,GameObject> playerShowerMap;
    List<GameObject> showedPrefabs = new List<GameObject>();
    List<RoomInfo> roomList = new List<RoomInfo>();

    public static LobbyManager Instance
    {
        set
        {
            if(instance == null)
            {
                instance = value;
            }
            else
            {
                Destroy(value);
            }
        }
        get
        {
            return instance;
        }
    }


    void Start()
    {
        Instance = this;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("서버 연결실패");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 연결성공");
        PhotonNetwork.JoinLobby();
        //Debug.Log(PhotonNetwork.InLobby);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("룸리스트 업데이트");
        //base.OnRoomListUpdate(roomList);
        foreach(RoomInfo info in roomList)
        {
            if(this.roomList.Contains(info))
            {
                this.roomList.Remove(info);
            }
            else
            {
                this.roomList.Add(info);
            }
        }
        Debug.Log(this.roomList.Count);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("룸 참가");
        roomSelectLayer.SetActive(false);
        roomLayer.SetActive(true);
    }

    public void CreatRoom()
    {
        Debug.Log("방생성");
        PhotonNetwork.CreateRoom(null, new RoomOptions { IsVisible = true, MaxPlayers = 8 });
        //ShowRoomList();
    }

    public void setNickName(string name)
    {
        PhotonNetwork.NickName = name;
    }

    public void ShowRoomList()
    {
        for(;showedPrefabs.Count != 0;)
        {
            Destroy(showedPrefabs[0]);
            showedPrefabs.Remove(showedPrefabs[0]);
        }
        
        for(int i = 0; i < roomList.Count; i++)
        {
            //표시창 생성 후 정보전달
            GameObject infoShower = Instantiate(roomInformationPrefab, roomListLayer.transform);
            showedPrefabs.Add(infoShower);
            infoShower.GetComponent<RoomInfoShower>().ShowRoomInfo(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject instance = Instantiate(playerNamePrefab, playerNameLayer.transform);
        instance.GetComponentInChildren<Text>().text = newPlayer.NickName;
        playerShowerMap.Add(newPlayer,instance);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerShowerMap[otherPlayer]);
        playerShowerMap.Remove(otherPlayer);
    }

    private void Update()
    {
        
    }
}
