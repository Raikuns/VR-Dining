using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Basket : MonoBehaviour {

    public bool correct;
    public bool LessThanWall;

    private int score;

    [SerializeField] protected int calorieLimit;

    [SerializeField] private List<Food> correctCalories = new List<Food>();
    [SerializeField] private List<Food> incorrectCalories = new List<Food>();
    
    public virtual void AddToList(Food _food)
    {
        if (correct)
        {
            correctCalories.Add(_food);
            score += 10;
            print("Answered Correctly");
            _food.gameObject.SetActive(false);
        }

        else if (!correct)
        {
            incorrectCalories.Add(_food);
            score -= 5;
            print("Wrong Answer");
            _food.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var food = other.gameObject.GetComponent<Food>();
        
    }
}
