using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine;


/// <summary>
/// This class contains the interaction between de foodbaskets in the scene and the 
/// food being thrown into it. 
/// </summary>
public class BasketBehaviour : MonoBehaviour {

    
    private FoodData dataManager;           //This will let us use the data manager that the buddy system needs/
    private SpeechManager speechManager;    //This will let us use the speech manager that the buddy has.
   
   
    void Start()
    {
        dataManager = GameObject.Find("DataManager").GetComponent<FoodData>();                  //This will give us acces to the datamanager.       
        speechManager = GameObject.Find("SpeechController").GetComponent<SpeechManager>();      //This will give us access to the speechmanager.

    }
    
    /// <summary>
    /// The method below checks for both foodbaskets if an object with the type: Food
    /// Enters their trigger. If that is true, it then checks wether it collides with the 
    /// like or dislike foodbasket. Once it gets through the if statement, it plays a random sound out of the
    /// respective speechmanager array. It then deactivates the object and adds the Gameobject ( food) into the right list.
    /// 
    /// Lastly, it sets a boolean true or false based on which basket it is in, using a foreach loop.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Food>() && gameObject.CompareTag("LikeBasket"))
        {
            speechManager.speeches.PlayPositive();
            Debug.Log("played sound");
            Debug.Log(other.gameObject + "Added to the LikeBasket");
            other.gameObject.SetActive(false);
            dataManager.PositveAddToList(other.gameObject);

            foreach (GameObject go in dataManager.LikeFood)
            {
                go.GetComponent<Food>().Liked = true;
            }
        }

        if (other.gameObject.GetComponent<Food>() && gameObject.CompareTag("DislikeBasket"))
        {
            dataManager.DislikeFood.Add(other.gameObject);
            speechManager.speeches.PlayNegative();
            Debug.Log(other.gameObject.name + " Added to the DislikeBasket");
            other.gameObject.SetActive(false);

            foreach (GameObject go in dataManager.DislikeFood)
            {
                go.GetComponent<Food>().Liked = false;
            }
        }
    }
}

