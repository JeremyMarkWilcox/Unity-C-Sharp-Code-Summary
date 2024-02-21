using System.Collections;
using UnityEngine;

public class EnemyProjectileShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform target;
    public float maxFireDistance = 20f;
    public float projectileSpeed = 10f;
    public float projectileDuration = 2f;
    public float shootInterval = 1f;
    public Transform firePoint;
    private float lastShootTime;
    private bool isShooting;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isShooting && Time.time - lastShootTime >= shootInterval)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            if (distanceToTarget <= maxFireDistance)
            {
                StartCoroutine(ShootCoroutine());
                lastShootTime = Time.time;
            }
        }
    }

    private IEnumerator ShootCoroutine()
    {
        isShooting = true;

        Vector2 shootDirection = (target.position - firePoint.position).normalized;

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        projectile.transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);

        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        projectileRb.velocity = shootDirection * projectileSpeed;

        Destroy(projectile, projectileDuration);

        yield return new WaitForSeconds(projectileDuration);

        rb.bodyType = RigidbodyType2D.Dynamic;

        isShooting = false;
    }
}