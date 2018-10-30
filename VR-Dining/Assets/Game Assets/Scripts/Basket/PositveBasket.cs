using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositveBasket : Basket {

	void OnTriggerEnter(Collider other)
    {
        var food = other.gameObject.GetComponent<Food>();
        if (food && gameObject.CompareTag("LikeBasket"))
        {
            positive = true;
            AddToList(food);
        }
    }
}
