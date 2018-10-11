using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine;

public class BasketBehaviour : MonoBehaviour {


    DataManager dataManager;
    Speech speech;
   
    void Start()
    {
        dataManager = GetComponent<DataManager>();
        speech = GetComponent<Speech>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Food>() && gameObject.CompareTag("LikeBasket"))
        {
            
            dataManager.LikeFood.Add(other.gameObject);
            speech.Positive();
            Debug.Log(other.gameObject + "Added to the LikeBasket");         
            other.gameObject.SetActive(false);

            foreach (GameObject go in dataManager.LikeFood)
            {
                go.GetComponent<Food>().Liked = true;
            }
        }

        if (other.gameObject.GetComponent<Food>() && gameObject.CompareTag("DislikeBasket"))
        {
            dataManager.DislikeFood.Add(other.gameObject);
            speech.Negative();
            Debug.Log(other.gameObject.name + " Added to the DislikeBasket");
            other.gameObject.SetActive(false);

            foreach (GameObject go in dataManager.DislikeFood)
            {
                go.GetComponent<Food>().Liked = false;
            }
        }
    }
}
