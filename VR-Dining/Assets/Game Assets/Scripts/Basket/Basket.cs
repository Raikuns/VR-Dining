using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Basket : MonoBehaviour {

    public bool correct;
    public bool LessThanWall;
    private GameManager gameManager;

    public int calorieLimit = 100;

    [SerializeField] private List<Food> correctCalories = new List<Food>();
    [SerializeField] private List<Food> incorrectCalories = new List<Food>();

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    public void AddToList(Food _food, int _score)
    {
        if (correct)
        {
            correctCalories.Add(_food);
            gameManager.UpdateScore(_score);
            Destroy(_food.gameObject);
        }

        else if (!correct)
        {
            incorrectCalories.Add(_food);
            gameManager.UpdateScore(_score);
            Destroy(_food.gameObject);
        }
    }
}
