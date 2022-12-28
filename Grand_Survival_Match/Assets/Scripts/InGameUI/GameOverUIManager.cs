using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{
    public RankingManager rank;
    public Text myScore;

    public void ShowGameResult(RankingData[] ranking)
    {
        rank.SetRank(ranking);
        for(int i = 0; i < ranking.Length; i++)
        {
            if (PhotonNetwork.PlayerList[ranking[i].id].NickName == PhotonNetwork.NickName)
            {
                myScore.text = PhotonNetwork.PlayerList[ranking[i].id].NickName + " " + ranking[i].kill + "/" + ranking[i].death;
            }
        }
    }
}
