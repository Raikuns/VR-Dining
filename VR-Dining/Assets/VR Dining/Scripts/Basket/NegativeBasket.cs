using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeBasket : Basket {

    void OnTriggerEnter(Collider other)
    {
        var food = other.gameObject.GetComponent<Food>();
        if (other.gameObject.GetComponent<Food>() && gameObject.CompareTag("DislikeBasket"))
        {
            positive = false;
            
            AddToList(food);
        }
    }
}
