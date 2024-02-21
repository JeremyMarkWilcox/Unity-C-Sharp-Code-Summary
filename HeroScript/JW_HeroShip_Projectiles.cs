using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JW_HeroShip_Projectiles : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float projectileDuration = 0.5f;
    public AudioSource laserSound;
    public float fireRate = 0.25f; 

    private float nextFireTime = 0f;

    private void JW_HeroShip_Projectiles_OnShoot()
    {
        Vector2 shootDirection = transform.up;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.AddForce(shootDirection * projectileSpeed, ForceMode2D.Impulse);

        Destroy(projectile, projectileDuration);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            laserSound.Play();
            JW_HeroShip_Projectiles_OnShoot();
            nextFireTime = Time.time + fireRate;
        }
    }
}
