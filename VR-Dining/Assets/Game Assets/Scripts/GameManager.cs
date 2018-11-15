using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI ScoreText;
    public int score = 0;
	// Use this for initialization
	void Start () {
		
	}
	
    public void UpdateScore(int _score)
    {
        score += _score;
        ScoreText.text = "Score: " + score;
    }
}
