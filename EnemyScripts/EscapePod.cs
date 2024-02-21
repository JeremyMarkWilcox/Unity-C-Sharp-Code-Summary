using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class EscapePod : MonoBehaviour
{
    public Rigidbody2D rb;
    float escapePodSpeed = 15f;

    void Start()
    {       
       StartEscapeSequence();       
    }

    async void StartEscapeSequence()
    {
        rb.velocity = new Vector2(0f, escapePodSpeed);
        await Task.Delay(1000);
        Destroy(gameObject);      
    }
}
