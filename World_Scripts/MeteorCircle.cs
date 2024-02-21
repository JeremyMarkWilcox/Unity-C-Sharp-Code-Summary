using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorCircle : MonoBehaviour
{
    public float rotationSpeed = .8f;
    public float radius = 3f;

    private Vector3 originalPosition;
    private float angle = 0f;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        angle += rotationSpeed * Time.deltaTime;
        float x = originalPosition.x + Mathf.Cos(angle) * radius;
        float y = originalPosition.y + Mathf.Sin(angle) * radius;
        float z = originalPosition.z; 
        transform.position = new Vector3(x, y, z);
    }
}