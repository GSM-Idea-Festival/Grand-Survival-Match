using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    Text text;
    GameManager gameManager;
   

    void Start()
    {
        text = GetComponent<Text>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        int timer;
        timer = (int)gameManager.GameOverTimer;
        text.text = (timer / 60).ToString() + " : "+ (timer % 60).ToString();
    }
}
