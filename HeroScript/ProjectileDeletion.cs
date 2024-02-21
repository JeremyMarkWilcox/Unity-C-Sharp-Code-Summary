using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDeletion : MonoBehaviour
{
    public List<string> destroyOnCollisionTags = new List<string>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (destroyOnCollisionTags.Contains(other.gameObject.tag))
        {
            Destroy(gameObject);
        }
    }
}
