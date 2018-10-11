using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class contains both lits that the food is being stored in
/// </summary>
public class FoodData : MonoBehaviour{


    public List<GameObject> LikeFood = new List<GameObject>();

  
    public List<GameObject> DislikeFood = new List<GameObject>();


    /// this method will add a GameObject to the LikeFood list.
    public void PositveAddToList(GameObject _food)
    {
        LikeFood.Add(_food);
    }

    /// this method will add a GameObject to the LikeFood list.
    public void NegativeAddToList(GameObject _food)
    {
        DislikeFood.Add(_food);
    }

}
