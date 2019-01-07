using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyMovement : MonoBehaviour
{

    private float mSpeed = 0.30f;
    private float resetPosition = -0.8f;
    // Update is called once per frame
    void Update()
    {
        LineMovement();
    }

    void LineMovement()
    {
        transform.Translate(Vector3.forward * (mSpeed * Time.deltaTime));

        if (transform.localPosition.x <= resetPosition)
        {
            transform.DetachChildren();
            Vector3 startingPos = new Vector3(1.63f, transform.position.y, transform.position.z);
            transform.position = startingPos;
        }
    }
   
}

