using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeBasket : Basket {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Food>() && gameObject.CompareTag("LikeBasket"))
        {
            positive = false;
            AddToList(other.gameObject);
        }
    }
}
