using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {

    [SerializeField] private List<ScriptableFood> food = new List<ScriptableFood>();
    bool spawned;
	// Use this for initialization
	void Start () {
        spawned = false;    
	}

    void SpawnFood()
    {
        StopAllCoroutines();
        if(spawned)
        {
            Instantiate(food[Random.Range(0, food.Count)].Model.gameObject, this.transform);
            StartCoroutine(SpawnTimer(3f));                       
        }       
    }

    public IEnumerator SpawnTimer(float timer)
    {
        spawned = true;
        yield return new WaitForSeconds(3f);
        SpawnFood();        
    }    
}
