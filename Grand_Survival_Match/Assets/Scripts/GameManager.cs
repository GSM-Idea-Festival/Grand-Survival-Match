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

    [SerializeField] GameObject canvas;

    [SerializeField] GameObject knightBottomInfo;
    [SerializeField] GameObject wizardBottomInfo;
    [SerializeField] GameObject gunnerBottomInfo;
    [SerializeField] GameObject spearManBottomInfo;
    [SerializeField] GameObject assassinBottomInfo;

    [SerializeField] GameObject hpBarLayerCanvas;
    [SerializeField] RankingManager rankingManager;
    [SerializeField] GameObject hpBarPrefab;

    public static RankingData[] ranking = new RankingData[8];

    GameObject player;


    [SerializeField] float gameOverTimer = 60;
    private void Awake()
    {
        PhotonPeer.RegisterType(typeof(RankingData), 128, RankingSerialization.SerializeRanking, RankingSerialization.DeserializeRanking);
        PhotonPeer.RegisterType(typeof(BuffWithTime), 129, BuffWithTimeSerialization.SerializeRanking, BuffWithTimeSerialization.DeserializeRanking);

    }

    void Start()
    {
        GameObject spawnPrefab = null;
        GameObject spawnBottomInfo = null;
        switch (MyCharacterType)
        {
            case CharacterType.Knight:
                spawnPrefab = knightPrefab;
                spawnBottomInfo = knightBottomInfo;
                break;
            case CharacterType.Wizard:
                spawnPrefab = wizardPrefab;
                spawnBottomInfo = wizardBottomInfo;
                break;
            case CharacterType.Assassin:
                spawnPrefab = assassinPrefab;
                spawnBottomInfo = assassinBottomInfo;
                break;
            case CharacterType.SpearMan:
                spawnPrefab = spearManPrefab;
                spawnBottomInfo = spearManBottomInfo;
                break;
            case CharacterType.Gunner:
                spawnPrefab = gunnerPrefab;
                spawnBottomInfo = gunnerBottomInfo;
                break;
        }

        player = PhotonNetwork.Instantiate(spawnPrefab.name, Vector3.zero, Quaternion.identity);
        Instantiate(spawnBottomInfo, canvas.transform);
        FindObjectOfType<Camera>().GetComponent<CameraFollow>().player = player.transform;





        //·©Å· °ü¸®
        for (int i = 0; i < 8; i++)
        {
            if (i < PhotonNetwork.PlayerList.Length)
            {
                ranking[i].id = i;
                ranking[i].kill = 0;
                ranking[i].death = 0;
            }
            else
            {
                ranking[i].id = -1;
                ranking[i].kill = 0;
                ranking[i].death = 0;
            }
        }


        RequestSendRankingData(0, 0);
    }

    public void Kill(string playerName)
    {
        photonView.RPC(nameof(AddRankingData), RpcTarget.All, playerName, 1, 0);
    }

    public void Death()
    {
        photonView.RPC(nameof(AddRankingData), RpcTarget.All, PhotonNetwork.NickName, 0, 1);
    }

    [PunRPC]
    void AddRankingData(string name, int kill, int death)
    {
        Debug.Log("AddRankingData");
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[ranking[i].id].NickName == name)
            {
                ranking[i].kill += kill;
                ranking[i].death += death;
                break;
            }
        }
        ShowRanking(ranking);
        //photonView.RPC(nameof(ShowRanking), RpcTarget.All, ranking);
    }

    public void spawnHpBar(GameObject target, string name)
    {
        CharacterHpBar bar = Instantiate(hpBarPrefab, hpBarLayerCanvas.transform).GetComponent<CharacterHpBar>();
        bar.trackingTarget = target;
        bar.SetPlayerName(name);
    }

    [PunRPC]
    void ShowRanking(RankingData[] ranking)
    {
        Debug.Log(ranking[0]);
        rankingManager.SetRank(ranking);
    }

    public void RequestSendRankingData(int kill, int death)
    {
        Debug.Log(photonView);
        photonView.RPC("AddRankingData", RpcTarget.MasterClient, PhotonNetwork.NickName, kill, death);
    }


    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameOverTimer -= Time.deltaTime;
            if (gameOverTimer <= 0)
            {
                photonView.RPC(nameof(GameOver), RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void GameOver()
    {
        PhotonNetwork.LoadLevel("GameOverScene");
    }

    public void RespawnRequest(GameObject player)
    {
        player.SetActive(false);
        StartCoroutine(Respawn(player));
    }

    IEnumerator Respawn(GameObject player)
    {
        yield return new WaitForSeconds(5);
        player.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length - 1)].transform.position;
        player.SetActive(true);
    }

    void SetPlayerFalse()
    {
        player.SetActive(false);
    }

    void SetPlayerTrue()
    {
        gameObject.SetActive(true);
    }
}
