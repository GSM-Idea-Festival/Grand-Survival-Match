using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviourPunCallbacks
{
    public RankingManager rank;
    public Text myScore;

    private void Start()
    {
        ShowGameResult(GameManager.ranking);
    }

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

    public void LeaveRoom()
    {
        Debug.Log("ÅðÀå½Ãµµ");
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
        }
        
    }

    public override void OnLeftLobby()
    {
        Debug.Log("ÅðÀå¼º°ø");
        SceneManager.LoadScene("LobbyScene");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }
}
