using System;
using System.Collections;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public int maxHits = 1;
    private int remainingLives;
    private bool canTrigger = true;
    private bool canTakeDamage = true;
    public float invincibilityDuration = 2f; 
    public SpriteRenderer spriteRenderer; 
    public Color damageColor = Color.red;
    private Color originalColor;
    public GameObject explosionEffect;
    public event Action OnBossDestroyed;
    public GameObject beamPrefab;
    public Transform firePoint;
    public Color warningColor = Color.yellow;
    private float warningDuration = 2f;
    public AudioSource bossdamagesound;
    public AudioSource bossdeath;
    public AudioSource bosslasersound;

    private void Start()
    {
        StartCoroutine(FireBeamRoutine());
        remainingLives = maxHits;
        originalColor = spriteRenderer.color;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canTrigger && canTakeDamage)
        {
            if (other.CompareTag("Player") || other.CompareTag("Projectile"))
            {
                bossdamagesound.Play();
                LoseLife();
                StartCoroutine(FlashDamageEffect());
            }
        }
    }

    void LoseLife()
    {
        remainingLives--;
        if (remainingLives <= 0)
        {
            Explode();
        }
        else
        {
            StartCoroutine(TemporaryInvincibility());
        }
    }

    IEnumerator TemporaryInvincibility()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(invincibilityDuration);
        canTakeDamage = true;
    }

    private IEnumerator FireBeamRoutine()
    {
        yield return new WaitForSeconds(20f);
        while (true)
        {
            StartCoroutine(WarningEffect());

            yield return new WaitForSeconds(warningDuration);

            bosslasersound.Play();
            for (int i = 0; i < 10; i++)
            {
                FireBeam();
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(10f); 
        }
    }

    private IEnumerator WarningEffect()
    {
        spriteRenderer.color = warningColor;

        yield return new WaitForSeconds(warningDuration);

        spriteRenderer.color = originalColor;
    }


    void FireBeam()
    {
        if (beamPrefab && firePoint)
        {
            GameObject beam = Instantiate(beamPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = beam.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(firePoint.up * -1000); 
            }
        }      
    }

    IEnumerator FlashDamageEffect()
    {
        SpriteRenderer[] allRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer renderer in allRenderers)
        {
            renderer.color = damageColor;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (SpriteRenderer renderer in allRenderers)
        {
            renderer.color = originalColor;
        }
    }


    void Explode()      
    {
        bossdeath.Play();

        if (explosionEffect)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnBossDestroyed?.Invoke();
    }
}
