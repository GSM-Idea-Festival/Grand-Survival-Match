using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataSender : MonoBehaviour
{
    BottomUIManager bottomUI;
    CharacterStats characterStats;

    void Start()
    {
        bottomUI = FindObjectOfType<BottomUIManager>();
        characterStats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        bottomUI.SetCoolTime(0, characterStats.QCooltime, 1);
    }
}
