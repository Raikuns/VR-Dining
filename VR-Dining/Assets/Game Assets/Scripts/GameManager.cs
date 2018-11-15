using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI ScoreText;
    public int Score = 0;

    void Start()
    {
        UpdateScore(Score);
    }

    public void UpdateScore(int _score)
    {
        Score += _score;
        ScoreText.text = "Score: " + Score;      
    }
}
