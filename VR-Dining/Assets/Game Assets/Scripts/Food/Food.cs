using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class the name of the food and wether it was liked or not.
/// This class is also to check wether a thrown object was of the type food or not.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Food : MonoBehaviour
{

    public string FoodName { get; set; }
    public bool Liked { get; set; }
    public int Calories { get; set; }

}