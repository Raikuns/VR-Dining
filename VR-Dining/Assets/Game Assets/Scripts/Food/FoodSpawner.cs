using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {

    [SerializeField] private List<ScriptableFood> food = new List<ScriptableFood>();
    bool spawned;
	// Use this for initialization
	void Start () {
        spawned = false;
        StartCoroutine(SpawnTimer());     
	}

    void SpawnFood()
    {
        StopAllCoroutines();
        if(spawned)
        {
            Instantiate(food[Random.Range(0, food.Count)].Model.gameObject, this.transform);
            StartCoroutine(SpawnTimer());                       
        }       
    }

    public IEnumerator SpawnTimer()
    {
        spawned = true;
        yield return new WaitForSeconds(3f);
        SpawnFood();        
    }    
}
