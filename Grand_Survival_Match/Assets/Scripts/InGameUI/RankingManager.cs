using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviourPun
{
    public GameObject rankInfoPrefab;
    public GameObject rankInfoLayer;
    GameObject[] rankInfoBuf = new GameObject[8];

    public void SetRank(RankingData[] rankList)
    {
        //버블정렬
        /*while (true)
        {
            for (int i = 0; i <PhotonNetwork.PlayerList.Length - 1; i++)
            {
                if (rankList[i].kill > rankList[i + 1].kill)
                {
                    RankingData temp = rankList[i];
                    rankList[i] = rankList[i + 1];
                    rankList[i + 1] = temp;
                }
            }
            for (int i = 0; i < PhotonNetwork.PlayerList.Length - 1; i++)
            {
                if (rankList[i].kill < rankList[i + 1].kill)
                {
                    continue;
                }
            }
            break;
        }*/

        foreach (GameObject g in rankInfoBuf)
        {
            if (g != null)
            {
                Destroy(g);
            }
        }
        rankInfoBuf = new GameObject[8];

        for (int j = PhotonNetwork.PlayerList.Length - 1; j >= 0; j--)
        {
            if (rankList[j].id == -1) {
                break;
            }
            GameObject g = Instantiate(rankInfoPrefab, rankInfoLayer.transform);
            Text rankName = g.GetComponentInChildren<Text>();
            rankName.text = (PhotonNetwork.PlayerList.Length - j) + ". " + PhotonNetwork.PlayerList[rankList[j].id].NickName + " " + rankList[j].kill + "/" + rankList[j].death;
            rankInfoBuf[j] = g;
        }
    }

    public void SendRanking(RankingData[] data)
    {
        photonView.RPC(nameof(RankingLoader), RpcTarget.All, data);
    }

    [PunRPC]
    void RankingLoader(RankingData[] ranking)
    {
        SetRank(ranking);
    }
}
