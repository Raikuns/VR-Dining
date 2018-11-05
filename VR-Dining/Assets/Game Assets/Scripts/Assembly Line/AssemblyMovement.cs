using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyMovement : MonoBehaviour
{

    float mSpeed = 0.25f;
    private float resetPosition = 2f;
    // Update is called once per frame
    void Update()
    {
        LineMovement();
    }

    void LineMovement()
    {
        transform.Translate(Vector3.down * (mSpeed * Time.deltaTime));

        if (transform.localPosition.z >= resetPosition)
        {
            transform.DetachChildren();
            Vector3 startingPos = new Vector3(transform.position.x, transform.position.y, -1.4f);
            transform.position = startingPos;
        }
    }
   
}

