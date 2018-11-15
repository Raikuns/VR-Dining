﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeBasket : Basket {

    void Start()
    {
        LessThanWall = false;
    }

    void OnTriggerEnter(Collider other)
    {
        var food = other.gameObject.GetComponent<Food>();
        if (food.scriptableFood.KcalAmount <= calorieLimit && !LessThanWall)
        {
            correct = false;
            AddToList(food, 26);
        }
        else if (food.scriptableFood.KcalAmount >= calorieLimit && LessThanWall)
        {
            correct = true;
            AddToList(food, -7);
        }
    }
}
