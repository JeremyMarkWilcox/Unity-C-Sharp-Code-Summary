using UnityEngine;
using System.Collections;
using System.Threading.Tasks;


public class EnemyBlueAI : MonoBehaviour
{
    public enum State
    {
        Patrolling,
        Chasing,
        Attacking,
        Death
    }

    public float speed = 5f;
    public Transform JW_Hero_ShipTransform;
    public float chaseTriggerDistance = 20f;

    public State currentState;
    private EnemyPatrollingState patrollingState;
    private EnemyChasingState chasingState;
    private EnemyAttackState attackState;

    public GameObject explosionPrefab;
    public GameObject escapePodPrefab;
    public Animator animator;
    public int maxHits = 1;
    public GameObject Lifeup;
    float dropChance = .1f;
    public AudioSource enemydamagesound;
    public AudioSource enemydeath;


    private int remainingLives;
    private bool canTrigger = true;
    private bool canTakeDamage = true;
    private Rigidbody2D rb;
    private bool isExploding = false;
    public EnemyBoss EnemyBoss;

    private void Start()
    {
        if (EnemyBoss != null)
        {
            EnemyBoss.OnBossDestroyed += HandleBossDestruction;
        }

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
                LoseLifeBlue();
                enemydamagesound.Play();
            }
            else if (other.CompareTag("Enemy"))
            {
                Vector2 directionAwayFromOther = (transform.position - other.transform.position).normalized;
                float repulsionStrength = 5f;
                GetComponent<Rigidbody2D>().AddForce(directionAwayFromOther * repulsionStrength, ForceMode2D.Impulse);
            }
        }
    }
    void LoseLifeBlue()
    {
        remainingLives--;
        if (remainingLives <= 0 && !isExploding)
        {
            isExploding = true;
            StartBlueDeath();
        }
    }
    async void ExplodeBlue()
    {     
        isExploding = true;
        await Task.Delay(250);
        DestroyGameObjectBlue();
    }
    async void DestroyGameObjectBlue()
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
                    StartBlueChasing();
                }
                break;

            case State.Chasing:
                chasingState.SetTarget(JW_Hero_ShipTransform);
                chasingState.UpdateState();
                if (Vector2.Distance(transform.position, JW_Hero_ShipTransform.position) < attackState.maxFireDistance)
                {
                    StartBlueAttacking();
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
                    StartBlueDeath();
                }
                break;

            case State.Death:

                break;
        }
    }
    public void StartBluePatrolling()
    {
        if (!isExploding)
        {
            currentState = State.Patrolling;
        }
        patrollingState.Enter();
    }

    public void StartBlueChasing()
    {
        if (!isExploding)
        {
            currentState = State.Chasing;
        }
    }
    public void StartBlueAttacking()
    {
        if (!isExploding)
        {
            currentState = State.Attacking;
        }
    }
    public void StartBlueDeath()
    {
        currentState = State.Death;
        ExplodeBlue();
    }


    public void MoveBlue(Vector2 direction)
    {
        direction.Normalize();
        rb.velocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle -= -90f;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void HandleBossDestruction()
    {
        isExploding = true;
        StartBlueDeath();
    }
}