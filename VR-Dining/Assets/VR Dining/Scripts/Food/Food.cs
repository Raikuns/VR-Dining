﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Food : MonoBehaviour {

    public string FoodName { get; set; }
    public bool Liked { get; set; }
}