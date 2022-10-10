using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI text;
    int playerScore;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "Score: 0";
    }

    public int GetScore()
    {
        return playerScore;
    }

    public void SetScore(int score)
    {
        playerScore = score;
        text.text = "Score: " + playerScore;
    }

    public void AddScore(int score)
    {
        playerScore += score;
        text.text = "Score: " + (playerScore);
    }
}
