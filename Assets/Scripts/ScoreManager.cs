using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreCount;
    [SerializeField] private Animator animTextScore;

    
    [SerializeField] private float smoothEffectPoint;
    private float currentScore;

    private float newScore;

    private void OnEnable()
    {
        PlayerController.AddPoint += AddScore;
    }
    private void OnDisable()
    {
        PlayerController.AddPoint -= AddScore;   
    }

    private void Update()
    {
        if(currentScore< newScore)
        {           
            currentScore += 3f;

            if(currentScore > newScore)
            {
               currentScore=newScore;
            }

            scoreCount.text = currentScore.ToString("0");
        }
            
    }
    private void AddScore(int score)
    {
        animTextScore.SetTrigger("addEffect");
        newScore = currentScore + score;
    }
}
