using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    private FoodSpawner foodSpawner;
    bool tutorialStarted = false;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
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
