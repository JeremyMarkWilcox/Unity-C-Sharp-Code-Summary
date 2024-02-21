using UnityEngine;
using System.Threading.Tasks;


public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Patrolling,
        Chasing,
        Attacking,
        Death
    }

    private Rigidbody2D rb;
    public float speed = 5f;
    public Transform JW_Hero_ShipTransform;
    public float chaseTriggerDistance = 20f;
    public State currentState;

    private EnemyPatrollingState patrollingState;
    private EnemyChasingState chasingState;
    private EnemyAttackState attackState;

    public GameObject explosionPrefab;
    public GameObject escapePodPrefab;
    public AudioSource enemydamagesound;
    public AudioSource enemydeath;
    public Animator animator;

    public GameObject Lifeup;
    float dropChance = 1f;
    private int remainingLives;
    private bool canTrigger = true;
    private bool canTakeDamage = true;
    public int maxHits = 1;
    private bool isExploding = false;

    private void Start()
    {
        patrollingState = GetComponent<EnemyPatrollingState>();
        chasingState = GetComponent<EnemyChasingState>();
        attackState = GetComponent<EnemyAttackState>();
        rb = GetComponent<Rigidbody2D>();
        remainingLives = maxHits;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canTrigger && canTakeDamage)
        {
            if (other.CompareTag("Player") || other.CompareTag("Projectile"))
            {
                animator.SetTrigger("EnemyHit");
                LoseLife();
                enemydamagesound.Play();
            }
        }
    }

    void LoseLife()
    {
        remainingLives--;
        if (remainingLives <= 0 && !isExploding)
        {
            isExploding = true;
            StartDeath();
        }
    }

    async void Explode()
    {
        isExploding = true;
        await Task.Delay(250);
        DestroyGameObject();
    }

    async void DestroyGameObject()
    {
        enemydeath.Play();
        float randomNumber = Random.value;

        if (randomNumber < dropChance)
        {
            Instantiate(Lifeup, transform.position, Quaternion.identity);
        }

        Instantiate(escapePodPrefab, transform.position, Quaternion.identity);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        await Task.Delay(50);
        Destroy(gameObject);
    }

    public void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                patrollingState.UpdateState();
                if (Vector2.Distance(transform.position, JW_Hero_ShipTransform.position) <= chaseTriggerDistance)
                {
                    StartChasing();
                }
                break;

            case State.Chasing:
                chasingState.SetTarget(JW_Hero_ShipTransform);
                chasingState.UpdateState();
                if (Vector2.Distance(transform.position, JW_Hero_ShipTransform.position) < chasingState.maintainDistance)
                {
                    StartAttacking();
                }
                break;

            case State.Attacking:
                rb.velocity = Vector2.zero;
                Vector2 directionToTarget = ((Vector2)JW_Hero_ShipTransform.position - (Vector2)transform.position).normalized;
                float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle - -90f, Vector3.forward);
                attackState.UpdateState(JW_Hero_ShipTransform.position);

                if (Vector2.Distance(transform.position, JW_Hero_ShipTransform.position) >= chasingState.maintainDistance)
                {
                    currentState = State.Chasing;
                }
                if (isExploding)
                {
                    StartDeath();
                }
                break;

            case State.Death:

                break;
        }
    }

    public void StartPatrolling()
    {
        if (!isExploding)
        {
            currentState = State.Patrolling;
        }
        patrollingState.Enter();
    }

    public void StartChasing()
    {
        if (!isExploding)
        {
            currentState = State.Chasing;
        }
    }

    public void StartAttacking()
    {
        if (!isExploding)
        {
            currentState = State.Attacking;
        }
    }

    public void StartDeath()
    {
        currentState = State.Death;
        Explode();
    }

    public void Move(Vector2 direction)
    {
        direction.Normalize();
        rb.velocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle -= -90f;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}