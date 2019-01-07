using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {


    [System.Serializable]
    public class Pool
    {
        public string tag;
        public Food foodPrefab;
        public int size;
    }
    [SerializeField] private List<ScriptableFood> food = new List<ScriptableFood>(); //List of the food objects that will be spawnded;
    bool spawned;   // Bool that is used for the Coroutines;

    
    
	// Use this for initialization
	void Start () {

 
        StartCoroutine(SpawnTimer(1f));  
	}

    void SpawnFood()
    {
        StopAllCoroutines();
        if(spawned)
        {
            Instantiate(food[Random.Range(0, food.Count)].Model.gameObject, this.transform);
            StartCoroutine(SpawnTimer(1.5f));                       
        }       
    }

    public IEnumerator SpawnTimer(float timer)
    {
        spawned = true;
        yield return new WaitForSeconds(timer);
        SpawnFood();        
    }    
}
