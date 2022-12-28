using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SocialPlatforms.Impl;
using ExitGames.Client.Photon;

public class GameManager : MonoBehaviourPun
{
    public static CharacterType MyCharacterType { private get; set; }

    [SerializeField] GameObject[] spawnPoints;

    [SerializeField] GameObject knightPrefab;
    [SerializeField] GameObject wizardPrefab;
    [SerializeField] GameObject gunnerPrefab;
    [SerializeField] GameObject spearManPrefab;
    [SerializeField] GameObject assassinPrefab;

    [SerializeField] RankingManager rankingManager;

    RankingData[] ranking = new RankingData[8];

    GameObject player;

    private void Awake()
    {
        PhotonPeer.RegisterType(typeof(RankingData), 128, RankingSerialization.SerializeRanking, RankingSerialization.DeserializeRanking);
    }

    void Start()
    {
        GameObject spawnPrefab = null;
        switch (MyCharacterType)
        {
            case CharacterType.Knight:
                spawnPrefab = knightPrefab;
                break;
            case CharacterType.Wizard:
                spawnPrefab = wizardPrefab;
                break;
            case CharacterType.Assassin:
                spawnPrefab = assassinPrefab;
                break;
            case CharacterType.SpearMan:
                spawnPrefab = spearManPrefab;
                break;
            case CharacterType.Gunner:
                spawnPrefab = gunnerPrefab;
                break;
        }
        player = PhotonNetwork.Instantiate(spawnPrefab.name,Vector3.zero,Quaternion.identity);
        FindObjectOfType<Camera>().GetComponent<CameraFollow>().player = player.transform;


        if (PhotonNetwork.IsMasterClient)
        {
            //·©Å· °ü¸®
            /*for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                ranking[i]._name = PhotonNetwork.PlayerList[i].NickName;
                ranking[i].kill = 0;
                ranking[i].death = 0;
            }*/
        }

        //RequestSendRankingData(0, 0);
    }

    [PunRPC]
    void AddRankingData(string name,int kill,int death)
    {
        for(int i=0;i< PhotonNetwork.PlayerList.Length; i++)
        {
            if (ranking[i]._name == name)
            {
                ranking[i].kill += kill;
                ranking[i].death += death;
                break;
            }
        }
        photonView.RPC("ShowRanking", RpcTarget.All,ranking);
    }



    [PunRPC]
    void ShowRanking(RankingData[] ranking)
    {
        rankingManager.SetRank(ranking);
    }

    public void RequestSendRankingData(int kill,int death)
    {
        Debug.Log(photonView);
        photonView.RPC("AddRankingData", RpcTarget.MasterClient, PhotonNetwork.NickName, kill, death);
    }

    public void RespawnRequest(GameObject player)
    {
        player.SetActive(false);
        StartCoroutine(Respawn(player));
    }

    IEnumerator Respawn(GameObject player)
    {
        yield return new WaitForSeconds(5);
        player.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
