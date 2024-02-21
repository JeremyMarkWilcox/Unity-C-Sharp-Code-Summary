using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class JW_HeroShipCollisions : MonoBehaviour
{
    public Animator animator;
    public string takeDamageTriggerName = "IsDead";
    public TextMeshProUGUI livesTextMeshPro;
    public GameObject stayInBoundsText;
    public int maxHits = 3;
    private int remainingLives;
    private bool canTrigger = true;
    private bool canTakeDamage = true; 
    private Rigidbody2D rb;
    public float minDamageVelocity = 10f;
    public float damageCooldown = 1.0f;
    public AudioSource crash;
    public AudioSource explodeSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        remainingLives = maxHits;
        UpdateLivesText();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canTrigger && canTakeDamage) 
        {
            if (other.CompareTag("Destroyable") || other.CompareTag("Wall"))
            {
                if (rb.velocity.magnitude > minDamageVelocity)
                {
                    LoseLife();
                    StartCoroutine(StartDamageCooldown());
                }
            }
            else if (other.CompareTag("Enemy"))
            {
                LoseLife();
                StartCoroutine(StartDamageCooldown());
            }


            else if (other.CompareTag("Boundary"))
            {
                ActivateStayInBoundsWithDelay();
            }
        }
    }

    void LoseLife()
    {
        remainingLives--;
        UpdateLivesText();

        if (remainingLives <= 0)
        {
            Explode();
        }
        else
        {
            if (animator != null)
            {
                animator.SetTrigger("TakingDamage");
                crash.Play();
            }
        }
    }

    void UpdateLivesText()
    {
        if (livesTextMeshPro != null)
        {
            livesTextMeshPro.text = "Remaining Hits: " + remainingLives;
        }
    }

    void Explode()
    {
        explodeSound.Play();
        DeactivateChildObjects();

        if (animator != null)
        {
            animator.SetTrigger("IsDead");
            Invoke("LoadGameOverScene", 1.0f);
        }
    }

    void LoadGameOverScene()
    {
        SceneManager.LoadScene("JW-GameOver");
    }

    public void AddExtraHits(int amount)
    {
        remainingLives += amount;
        UpdateLivesText();
    }

    private IEnumerator ActivateStayInBoundsText()
    {
        stayInBoundsText.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        stayInBoundsText.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        canTrigger = true;
    }

    public void ActivateStayInBoundsWithDelay()
    {
        if (canTrigger)
        {
            canTrigger = false;
            StartCoroutine(ActivateStayInBoundsText());
        }
    }

    private IEnumerator StartDamageCooldown()
    {
        canTakeDamage = false; 
        yield return new WaitForSeconds(damageCooldown); 
        canTakeDamage = true; 
    }
    void DeactivateChildObjects()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}

