using System.Collections;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnInterval = 30f;

    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }
    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);          
        }
    }
    void SpawnEnemy()
    {
        Instantiate(objectToSpawn, transform.position, transform.rotation);
    }
}
