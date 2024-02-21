using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame;

public class JW_DestroyableObjects : MonoBehaviour
{
    public List<string> destroyOnCollisionTags = new List<string>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (destroyOnCollisionTags.Contains(other.gameObject.tag))
        {
            Destroy(gameObject);
            JW_LargeMeteors largeMeteors = GetComponent<JW_LargeMeteors>();
          
            if (largeMeteors != null)
            {
                largeMeteors.SpawnSmallerMeteors();
            }           
        }
    }
}
