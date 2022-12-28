using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //싱글톤
    static LobbyManager instance;

    //잡다한 UI 연결
    public GameObject roomSelectLayer;
    public GameObject roomLayer;
    public GameObject roomInformationPrefab;
    public GameObject roomListLayer;
    //public GameObject playerListLayer;
    public GameObject playerNamePrefab;
    public GameObject playerNameLayer;
    public GameObject HostingButton;
    public GameObject GameStartButton;
    public GameObject RoomnameText;
    Text myCharacterText;

    //룸 내에서 보이는 플레이어리스트 목록
    Dictionary<Player,GameObject> playerShowerMap = new Dictionary<Player, GameObject>();
    //로비에서 보이는 룸리스트 목록
    List<GameObject> showedRoomPrefabs = new List<GameObject>();
    //로비에서 룸리스트 보여줄때 쓰는 정보
    List<RoomInfo> roomList = new List<RoomInfo>();

    //싱글톤
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

    //끊기면 재시도
    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        //Debug.Log(PhotonNetwork.InLobby);
    }
    
    //룸리스트 받아오기
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
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
    }

    public override void OnJoinedRoom()
    {
        //로비화면에서 방화면으로 넘어가기
        roomSelectLayer.SetActive(false);
        roomLayer.SetActive(true);
        Debug.Log(PhotonNetwork.LocalPlayer);
        //플레이어리스트 출력
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            GameObject instance = Instantiate(playerNamePrefab, playerNameLayer.transform);
            instance.GetComponent<PlayerName>().nameText.text = player.NickName;
            instance.GetComponentInChildren<Dropdown>().interactable = player.NickName == PhotonNetwork.LocalPlayer.NickName;
            if (player.NickName != PhotonNetwork.LocalPlayer.NickName)
            {
                Destroy(instance.GetComponent<PlayerName>().characterText);
            }
            playerShowerMap.Add(player, instance);
            if(player.NickName == PhotonNetwork.NickName)
            {
                myCharacterText = instance.GetComponent<PlayerName>().characterText;
            }
        }

        //방장만 게임시작버튼 활성화
        if (!PhotonNetwork.IsMasterClient)
        {
            GameStartButton.SetActive(false);
            HostingButton.SetActive(false);
        }

        RoomnameText.GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name+"님의 게임";
    }

    //버튼 연결용 wrap
    public void CreatRoom()
    {
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName, new RoomOptions { IsVisible = true, MaxPlayers = 8/* ,IsOpen = false*/});
        //ShowRoomList();
    }

    //버튼 연결용 wrap
    public void setNickName(string name)
    {
        PhotonNetwork.NickName = name;
    }

    public void ShowRoomList()
    {
        //이미 출력된 목록 제거
        for(;showedRoomPrefabs.Count != 0;)
        {
            Destroy(showedRoomPrefabs[0]);
            showedRoomPrefabs.Remove(showedRoomPrefabs[0]);
        }
        
        //다시 출력
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].PlayerCount != 0 && roomList[i].PlayerCount != roomList[i].MaxPlayers)
            {
                //표시창 생성 후 정보전달
                GameObject infoShower = Instantiate(roomInformationPrefab, roomListLayer.transform);
                showedRoomPrefabs.Add(infoShower);
                infoShower.GetComponent<RoomInfoShower>().ShowRoomInfo(roomList[i]);
            }
        }
    }

    //다른 플레이어 참가시 목록에 표시
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject instance = Instantiate(playerNamePrefab, playerNameLayer.transform);
        instance.GetComponentInChildren<Text>().text = newPlayer.NickName;
        instance.GetComponentInChildren<Dropdown>().interactable = false;
        playerShowerMap.Add(newPlayer,instance);
    }

    //플레이어 퇴장시 목록에서 제거
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerShowerMap[otherPlayer]);
        playerShowerMap.Remove(otherPlayer);
    }

    public override void OnLeftRoom()
    {
        //방 화면에서 로비화면으로 돌아가기
        roomLayer.SetActive(false);
        roomSelectLayer.SetActive(true);

        //출력된 플레이어리스트 초기화
        foreach(GameObject g in playerShowerMap.Values)
        {
            Destroy(g);
        }
        playerShowerMap = new Dictionary<Player, GameObject>();

        //방목록 출력
        ShowRoomList();
    }

    //버튼연결용 wrap
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    //버튼연결용 wrap
    public void Hosting()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }

    //버튼연결용 wrap
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            switch (myCharacterText.text)
            {
                case "전사":
                    GameManager.MyCharacterType = CharacterType.Knight;
                    break;
                case "마법사":
                    GameManager.MyCharacterType = CharacterType.Wizard;
                    break;
                case "창술사":
                    GameManager.MyCharacterType = CharacterType.SpearMan;
                    break;
                case "저격수":
                    GameManager.MyCharacterType = CharacterType.Gunner;
                    break;
                case "암살자":
                    GameManager.MyCharacterType = CharacterType.Assassin;
                    break;

            }
            photonView.RPC(nameof(Load), RpcTarget.All);
        }
    }

    [PunRPC]
    void Load()
    {
        PhotonNetwork.LoadLevel("InGameScene");
    }
}
