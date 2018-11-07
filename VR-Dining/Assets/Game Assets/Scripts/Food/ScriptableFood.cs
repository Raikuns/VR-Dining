using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "FoodTypes/Food")]
[RequireComponent(typeof(Rigidbody))]
public class ScriptableFood : ScriptableObject {

    public string FoodName;
    public int KcalAmount;
    public GameObject Model;
}
