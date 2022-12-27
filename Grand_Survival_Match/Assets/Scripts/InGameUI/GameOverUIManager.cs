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
        rank.setRank(ranking);
        for(int i = 0; i < ranking.Length; i++)
        {
            if (ranking[i].name == PhotonNetwork.NickName)
            {
                myScore.text = ranking[i].name + " " + ranking[i].kill + "/" + ranking[i].death;
            }
        }
    }
}
