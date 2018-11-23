using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    [SerializeField] private FoodSpawner foodSpawner;
    [SerializeField] private Light tutorialLight;
    private float luminositySteps = 0.05f;
    private float minIntensity = 7f;
    private float maxIntensity = 12f;
    private float shineDuration = 0.00000005f;
    private float counter = 0f;

    void Start()
    {
        tutorialLight.intensity = Random.Range(minIntensity, maxIntensity);
        StartCoroutine(ChangeIntensity());
        
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(other);        
        foodSpawner.StartCoroutine(foodSpawner.SpawnTimer(8f));
        Destroy(this.gameObject);
        Destroy(tutorialLight.gameObject);
    }

    private IEnumerator ChangeIntensity()
    {
        while (true)
        {
            while (tutorialLight.intensity <= maxIntensity)
            {
                tutorialLight.intensity += luminositySteps; // increase the firefly intensity / fade in
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(shineDuration); // wait 3 seconds

            while (tutorialLight.intensity > minIntensity)
            {
                tutorialLight.intensity -= luminositySteps;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
