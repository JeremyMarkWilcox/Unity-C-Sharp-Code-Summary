using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JW_MeteorMovement : MonoBehaviour
{
    public float floatForce = 10f;

    private Rigidbody2D rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    void FixedUpdate()
    {
        Vector2 floatDirection = Random.insideUnitCircle.normalized; 
        rb.AddForce(floatDirection * floatForce);
    }
}

