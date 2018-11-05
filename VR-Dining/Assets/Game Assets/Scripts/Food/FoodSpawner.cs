using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {

    [SerializeField] private List<Food> food = new List<Food>();
    bool spawned;
	// Use this for initialization
	void Start () {

        spawned = false;
        StartCoroutine(SpawnTimer());     
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnFood()
    {
        var _food = gameObject.GetComponent<Food>();
        StopAllCoroutines();
        if(spawned)
        {
            Instantiate(food[Random.Range(0,food.Count)], gameObject.transform);                
            StartCoroutine(SpawnTimer());             
            
        }
       
    }


    public IEnumerator SpawnTimer()
    {
        spawned = true;

        yield return new WaitForSeconds(3);
        SpawnFood();
        
    }

    
}
