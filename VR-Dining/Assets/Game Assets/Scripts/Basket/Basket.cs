using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Basket : MonoBehaviour
{

    protected bool correct;
    public bool LessThanWall;
    private GameManager gameManager;

    public int calorieLimit = 100;

    [SerializeField] private List<Food> correctCalories = new List<Food>();
    [SerializeField] private List<Food> incorrectCalories = new List<Food>();

    void Start()
    {
        //gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    public void AddToList(Food _food, int _score)
    {
        if (correct)
           correctCalories.Add(_food);        
        else       
            incorrectCalories.Add(_food);
        Destroy(_food.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        var food = other.gameObject.GetComponent<Food>();
        if (food.scriptableFood.KcalAmount <= calorieLimit && !LessThanWall)
        {
            correct = false;
            AddToList(food, 26);
        }
        if (food.scriptableFood.KcalAmount >= calorieLimit && !LessThanWall)
        {
            correct = true;
            AddToList(food, -7);
        }

        if (food.scriptableFood.KcalAmount <= calorieLimit && LessThanWall)
        {
            correct = true;
            AddToList(food, 26);
        }
        else if (food.scriptableFood.KcalAmount >= calorieLimit && LessThanWall)
        {
            correct = false;
            AddToList(food, -7);
        }
    }
}

     
       
