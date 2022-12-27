using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GameManager : MonoBehaviour
{
    public static CharacterType MyCharacterType { private get; set; }

    public GameObject[] spawnPoints;

    public GameObject knightPrefab;
    public GameObject wizardPrefab;
    public GameObject gunnerPrefab;
    public GameObject spearManPrefab;
    public GameObject assassinPrefab;

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
        PhotonNetwork.Instantiate(spawnPrefab.name,Vector3.zero,Quaternion.identity);
    }

    public void RespawnRequest(GameObject player)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
