using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTargets : MonoBehaviour {

    public float frequency = 1f;
    public float amplitude = 0.5f;

    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();
    Vector3 startPos = new Vector3();
    // Use this for initialization
    void Start () {
        startPos = transform.position;
        posOffset = startPos;
        amplitude = Random.Range(0.02f, 0.15f);
	}
	
	// Update is called once per frame
	void Update () {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI ) * amplitude;

        transform.position = tempPos;
    }
}
