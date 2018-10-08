using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine;

public class BasketBehaviour : MonoBehaviour {


    public List<GameObject> LikeFood = new List<GameObject>();
    public List<GameObject> DislikeFood = new List<GameObject>();

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Food>() && gameObject.CompareTag("LikeBasket"))
        {
            LikeFood.Add(other.gameObject);
            Debug.Log(other.gameObject + "Added to the LikeBasket");         
            other.gameObject.SetActive(false);
            
        }

        if (other.gameObject.GetComponent<Food>() && gameObject.CompareTag("DislikeBasket"))
        {
            DislikeFood.Add(other.gameObject);
            Debug.Log(other.gameObject.name + " Added to the DislikeBasket");
            other.gameObject.SetActive(false);
        }

    }
}
