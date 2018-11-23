using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositveBasket : Basket {

    void Start()
    {
        LessThanWall = true;
    }

    void OnTriggerEnter(Collider other)
    {
        var food = other.gameObject.GetComponent<Food>();
        if (food.scriptableFood.KcalAmount <= calorieLimit && LessThanWall)
        {
            correct = true;
            AddToList(food,26);
        }
        else if(food.scriptableFood.KcalAmount >= calorieLimit && LessThanWall)
        {
            correct = false;
            AddToList(food,-7);
        }
    }
}
