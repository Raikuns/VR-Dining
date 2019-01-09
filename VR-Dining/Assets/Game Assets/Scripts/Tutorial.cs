using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    private FoodSpawner foodSpawner;
    bool tutorialStarted = false;
    private SteamVR_TrackedObject t_Object;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)t_Object.index); }
    }

    private void Awake()
    {
        t_Object = GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
        foodSpawner= GameObject.FindObjectOfType<FoodSpawner>().GetComponent<FoodSpawner>();
        tutorialStarted = false;
    }

    void Update()
    {
        if (Controller.GetHairTriggerDown() && tutorialStarted == false)
        {
            print("Pressed");
            TriggerPressed();
            
        }
    }

    void TriggerPressed()
    {
        tutorialStarted = true;
        StartCoroutine(foodSpawner.SpawnTimer(1f));
        
    }






}
