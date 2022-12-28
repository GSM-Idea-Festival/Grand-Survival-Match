using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //�̱���
    static LobbyManager instance;

    //����� UI ����
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

    //�� ������ ���̴� �÷��̾��Ʈ ���
    Dictionary<Player,GameObject> playerShowerMap = new Dictionary<Player, GameObject>();
    //�κ񿡼� ���̴� �븮��Ʈ ���
    List<GameObject> showedRoomPrefabs = new List<GameObject>();
    //�κ񿡼� �븮��Ʈ �����ٶ� ���� ����
    List<RoomInfo> roomList = new List<RoomInfo>();

    //�̱���
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

    //����� ��õ�
    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        //Debug.Log(PhotonNetwork.InLobby);
    }
    
    //�븮��Ʈ �޾ƿ���
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
        //�κ�ȭ�鿡�� ��ȭ������ �Ѿ��
        roomSelectLayer.SetActive(false);
        roomLayer.SetActive(true);
        Debug.Log(PhotonNetwork.LocalPlayer);
        //�÷��̾��Ʈ ���
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

        //���常 ���ӽ��۹�ư Ȱ��ȭ
        if (!PhotonNetwork.IsMasterClient)
        {
            GameStartButton.SetActive(false);
            HostingButton.SetActive(false);
        }

        RoomnameText.GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name+"���� ����";
    }

    //��ư ����� wrap
    public void CreatRoom()
    {
        PhotonNetwork.CreateRoom(PhotonNetwork.NickName, new RoomOptions { IsVisible = true, MaxPlayers = 8/* ,IsOpen = false*/});
        //ShowRoomList();
    }

    //��ư ����� wrap
    public void setNickName(string name)
    {
        PhotonNetwork.NickName = name;
    }

    public void ShowRoomList()
    {
        //�̹� ��µ� ��� ����
        for(;showedRoomPrefabs.Count != 0;)
        {
            Destroy(showedRoomPrefabs[0]);
            showedRoomPrefabs.Remove(showedRoomPrefabs[0]);
        }
        
        //�ٽ� ���
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].PlayerCount != 0 && roomList[i].PlayerCount != roomList[i].MaxPlayers)
            {
                //ǥ��â ���� �� ��������
                GameObject infoShower = Instantiate(roomInformationPrefab, roomListLayer.transform);
                showedRoomPrefabs.Add(infoShower);
                infoShower.GetComponent<RoomInfoShower>().ShowRoomInfo(roomList[i]);
            }
        }
    }

    //�ٸ� �÷��̾� ������ ��Ͽ� ǥ��
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject instance = Instantiate(playerNamePrefab, playerNameLayer.transform);
        instance.GetComponentInChildren<Text>().text = newPlayer.NickName;
        instance.GetComponentInChildren<Dropdown>().interactable = false;
        playerShowerMap.Add(newPlayer,instance);
    }

    //�÷��̾� ����� ��Ͽ��� ����
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerShowerMap[otherPlayer]);
        playerShowerMap.Remove(otherPlayer);
    }

    public override void OnLeftRoom()
    {
        //�� ȭ�鿡�� �κ�ȭ������ ���ư���
        roomLayer.SetActive(false);
        roomSelectLayer.SetActive(true);

        //��µ� �÷��̾��Ʈ �ʱ�ȭ
        foreach(GameObject g in playerShowerMap.Values)
        {
            Destroy(g);
        }
        playerShowerMap = new Dictionary<Player, GameObject>();

        //���� ���
        ShowRoomList();
    }

    //��ư����� wrap
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    //��ư����� wrap
    public void Hosting()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }

    //��ư����� wrap
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            switch (myCharacterText.text)
            {
                case "����":
                    GameManager.MyCharacterType = CharacterType.Knight;
                    break;
                case "������":
                    GameManager.MyCharacterType = CharacterType.Wizard;
                    break;
                case "â����":
                    GameManager.MyCharacterType = CharacterType.SpearMan;
                    break;
                case "���ݼ�":
                    GameManager.MyCharacterType = CharacterType.Gunner;
                    break;
                case "�ϻ���":
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
