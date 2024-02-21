using System.Collections;
using UnityEngine;

public class EnemyAttackState : MonoBehaviour
{
    private EnemyAI enemyAI;
    public GameObject projectilePrefab;
    public Transform target;
    private AudioSource audioSource;
    public float maxFireDistance = 20f;
    public float projectileSpeed = 10f;
    public float projectileDuration = 2f;
    public float shootInterval = 1f;
    public Transform firePoint;
    private float lastShootTime;
    public float aimingTime;

    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        audioSource = GetComponent<AudioSource>();
        lastShootTime = -shootInterval;
    }

    public void UpdateState(Vector2 targetPosition)
    {
        if (Time.time - lastShootTime >= shootInterval)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            if (distanceToTarget <= maxFireDistance)
            {
                if (aimingTime <= 0f)
                {
                    Shoot(); // 
                    aimingTime = .4f;
                }
                else
                {
                    aimingTime -= Time.deltaTime; 
                }
            }
        }
    }

    private void Shoot()
    {
        audioSource.Play();
        lastShootTime = Time.time;


        Vector2 shootDirection = (target.position - firePoint.position).normalized;       

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        projectile.transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);

        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        projectileRb.velocity = shootDirection * projectileSpeed;

        Destroy(projectile, projectileDuration);
    }
}
