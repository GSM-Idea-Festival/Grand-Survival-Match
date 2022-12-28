using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingManager : MonoBehaviour
{
    public GameObject rankInfoPrefab;
    public GameObject rankInfoLayer;
    GameObject[] rankInfoBuf = new GameObject[8];

    public void SetRank(RankingData[] rankList)
    {
        //버블정렬
        while (true)
        {
            for (int i = 0; i < rankList.Length - 1; i++)
            {
                if (rankList[i].kill > rankList[i + 1].kill)
                {
                    RankingData temp = rankList[i];
                    rankList[i] = rankList[i + 1];
                    rankList[i + 1] = temp;
                }
            }
            for (int i = 0; i < rankList.Length - 1; i++)
            {
                if (rankList[i].kill < rankList[i + 1].kill)
                {
                    continue;
                }
            }
            break;
        }

        foreach (GameObject g in rankInfoBuf)
        {
            if (g != null)
            {
                Destroy(g);
            }
        }
        rankInfoBuf = new GameObject[8];

        for (int j = rankList.Length - 1; j >= 0; j--)
        {
            GameObject g = Instantiate(rankInfoPrefab, rankInfoLayer.transform);
            Text rankName = g.GetComponentInChildren<Text>();
            rankName.text = (rankList.Length - j) + ". " + rankList[j]._name + " " + rankList[j].kill + "/" + rankList[j].death;
            rankInfoBuf[j] = g;
        }
    }
}
