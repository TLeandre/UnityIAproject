using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDetection : MonoBehaviour
{
    [SerializeField]
    private int score = 0;

    public Text textScore;

    void Start()
    {
      textScore.text = "Score actuel : " + score;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if ( col.tag == "pipePassage")
        {
            score++;
            textScore.text = "Score actuel : " + score;
        }
    }
}
