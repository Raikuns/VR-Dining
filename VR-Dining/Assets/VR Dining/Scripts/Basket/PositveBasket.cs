using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositveBasket : Basket {

	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Food>() && gameObject.CompareTag("LikeBasket"))
        {
            positive = true;
            AddToList(other.gameObject);
        }
    }
}
