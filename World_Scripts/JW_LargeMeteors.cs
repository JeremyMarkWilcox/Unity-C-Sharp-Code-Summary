using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class JW_LargeMeteors : MonoBehaviour
    {
        public GameObject smallMeteorPrefab;
        public GameObject mediumMeteorPrefab;
        public int numberOfMediumMeteors = 3;
        public int numberOfSmallMeteors = 4;
        public float spawnRadius = 1.0f; 

        public void SpawnSmallerMeteors()
        {          
            for (int i = 0; i < numberOfSmallMeteors; i++)
            {
                Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

                Instantiate(smallMeteorPrefab, spawnPosition, Quaternion.identity);
            }
            
            for (int i = 0; i < numberOfMediumMeteors; i++)
            {
                Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

                Instantiate(mediumMeteorPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}