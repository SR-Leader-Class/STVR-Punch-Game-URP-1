using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameControllerPGXL : MonoBehaviour
{


    static public int Score;
    
    public int score;
    public Text scoreText;

    void Start()
    {
        
    }

    
    void Update()
    {
        score = Score;
        scoreText.text = "Score:" + score;
    }
}
