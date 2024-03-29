# Unity/ C# Code Summary On Live Project
I created an upgraded 2D version of Space Invaders with the Unity Engine, written in C#

## Introduction

In my final two courses, I delved into game development with Unity/C# and Unreal/C++. Each course culminated in a two-week live project, where I contributed an original game to a larger collaborative effort. My practical understanding of Agile/Scrum methodologies deepened through daily stand-ups and weekly code retrospectives, which bolstered accountability and fine-tuned our code management practices. Mastery of version control was essential due to the frequent pull requests and updates our team managed daily.

Designing my own video game was an invaluable opportunity to explore Unity's game engine intricacies and harness specific C# libraries for game functionality. Debugging and iterative development were constant companions as I expanded the game's features. Among the project's milestones, I am proud to have been the first student in the Bootcamp to implement a finite state machine successfully and the second to engineer a boss battle.

Enclosed are snippets from my code contributions, with the complete codebase accessible in the accompanying files.

## Play On My Personal Website

Here is a link to my portfolio where you can play the mini game in full:

https://www.jeremy-wilcox.com/

## Skills Aqcuired:

1. Advanced C# and Object-Oriented Programming: Proficiency in using classes, inheritance, polymorphism, and interfaces to organize and manage game entities and behaviors efficiently.

2. Unity Engine Mastery: Comprehensive understanding of Unity's core features, including physics, animations, particle systems, UI, and scene management, to create immersive gameplay experiences.

3. Gameplay Mechanics Implementation: Skill in developing complex gameplay mechanics, such as finite state machines for AI behavior, pathfinding, player controls, and projectile dynamics.

4. AI Development: Ability to design intelligent and adaptive enemy behavior using AI techniques, enabling dynamic interactions and challenges within the game environment.

5. Debugging and Performance Optimization: Competence in identifying and resolving bugs, along with optimizing game performance to ensure smooth and responsive gameplay.

6. Version Control with Git: Familiarity with using Git for source code management, facilitating team collaboration, and efficiently handling project iterations.

7. Agile/Scrum Methodologies: Experience in applying Agile principles and Scrum practices to game development, enhancing team collaboration, project planning, and delivery processes.

8. Creative Game Design: Skills in level design, challenge creation, and balancing game mechanics to craft engaging and enjoyable player experiences.

9. Project Management: Demonstrated ability to manage development timelines, prioritize tasks, and iterate on game features based on feedback or testing results.
    
## Enemy AI/ Finite State Machine

![EnemyAI](https://github.com/JeremyMarkWilcox/Unity-C-Sharp-Code-Summary/assets/150622088/7d8ded98-20b3-44ed-81fe-451b4ea68919)


The EnemyAI component implements a finite state machine (FSM) to dynamically control enemy behavior in four key states: Patrolling, Chasing, Attacking, and Death. This system allows the enemy AI to seamlessly transition between behaviors based on player proximity and interactions:

**Initialization:** Begins in Patrolling, roaming the area.

**Player Detection:** Transitions to Chasing when the player is detected within a certain range.

**Close Proximity:** Switches to Attacking to engage the player upon getting close enough.

**Taking Damage:** Handles taking damage and triggers the Death state upon health depletion, leading to an explosion effect and the enemy's destruction.
Utilizing Unity's physics and asynchronous programming, the FSM enhances gameplay with adaptive and challenging enemy behaviors. Each state is modular, encapsulated in its own script for clarity and ease of modification.

```
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
```
## Patroll State

![PatrollingState](https://github.com/JeremyMarkWilcox/Unity-C-Sharp-Code-Summary/assets/150622088/c547acd4-f892-4031-8929-7baf623e33c2)

The Enemy Patrolling State script is a crucial component of the enemy AI, enabling it to patrol predetermined waypoints in the game environment. This state represents the enemy's default behavior, allowing it to autonomously navigate through specific paths or areas until another condition triggers a state change.

### Core Functionality

**Waypoint Navigation:** The enemy follows a set of predefined waypoints, moving from one to the next in a loop. This behavior is fundamental for simulating routine patrols.

**Dynamic Movement:** Upon reaching a waypoint, the enemy automatically proceeds to the next one, ensuring continuous movement along the patrol route.

**Seamless Transitions:** The transition between waypoints is handled smoothly, with the enemy adjusting its direction and movement towards the next target immediately upon reaching a waypoint.
Key Features

**Waypoints Array:** A public array of Transforms represents the waypoints. The enemy cycles through these points in sequential order, creating a predictable yet customizable patrol path.

**State Entry:** The Enter method initializes the patrolling behavior by directing the enemy towards the first waypoint.

**Update Logic:** Within UpdateState, the script checks if the enemy has reached the current waypoint. If so, it advances to the next waypoint; otherwise, it continues moving towards the current target.

**Proximity Check:** The ReachedWaypoint method determines when the enemy is close enough to a waypoint to consider it "reached," using a small threshold distance to accommodate for floating-point precision errors.

**Movement and Orientation:** The movement towards each waypoint is calculated by determining the direction vector from the enemy's current position to the target waypoint. The enemy's movement is then updated to proceed in this direction, leveraging the Move method defined in the EnemyAI script, which abstracts the details of motion and rotation towards the target.

**Integration with EnemyAI:** The script interacts closely with the EnemyAI component, which manages state transitions and provides a method for movement. This modular design allows for easy adjustments to patrol behavior or integration of additional states into the enemy's AI.



```
using UnityEngine;

public class EnemyPatrollingState : MonoBehaviour
{
    private EnemyAI enemyAI;

    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    public void Enter()
    {
        GoToNextWaypoint();
    }

    public void UpdateState()
    {
        if (ReachedWaypoint())
        {
            GoToNextWaypoint();
        }
        else
        {
            MoveTowardsWaypoint();
        }
    }

    private bool ReachedWaypoint()
    {
        float distance = Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position);
        return distance < 0.1f;
    }

    private void GoToNextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    private void MoveTowardsWaypoint()
    {
        Vector2 targetPosition = waypoints[currentWaypointIndex].position;
        Vector2 currentPosition = transform.position;
        Vector2 direction = (targetPosition - currentPosition).normalized;

        enemyAI.Move(direction);
    }
}
```

## Chase State

![ChaseState](https://github.com/JeremyMarkWilcox/Unity-C-Sharp-Code-Summary/assets/150622088/9f13b94b-45d2-484d-ba96-47eaa92685b7)


The Chasing state, as defined in the Enemy Chasing State script, orchestrates the enemy's behavior when it has detected and is actively pursuing the player. This script is a vital component of the enemy AI's ability to engage with the player in a dynamic and challenging manner.

### Functionality

**Target Acquisition:** The script requires a target, typically the player, which is set through the SetTarget method. This allows the enemy to dynamically focus on the player regardless of their position in the game world.

**Movement:** Upon each frame update, the enemy calculates the direction to the target and moves towards it at a specified speed (speed), ensuring a continuous pursuit.

**Distance Maintenance:** While the script doesn't explicitly enforce maintaining a set distance from the player (maintainDistance), this value could be used to modify behavior, such as transitioning to an attack state or adjusting pursuit speed to avoid getting too close or too far.

**Rotation:** The enemy rotates to face the player as it moves, creating a more lifelike and engaging pursuit. This rotation is smoothly handled to always orient the enemy towards the player, enhancing visual fidelity.

### Key Aspects

**Adaptive Pursuit:** The script ensures the enemy can adapt its chase in real-time to the player's movements, providing a realistic and responsive AI behavior.

**Error Handling:** It includes a safeguard against a missing target, which logs a warning. This feature aids in debugging and ensures the game remains robust against unexpected errors.

**Efficient Targeting:** Through normalization of the direction vector and the use of quaternion rotation, the script ensures efficient and smooth pursuit mechanics.

```
using UnityEngine;

public class EnemyChasingState : MonoBehaviour
{
    public float speed = 5f;
    public float maintainDistance = 10f; 
    private Transform target; 

    public void UpdateState()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned in EnemyChasingState!");
            return;
        }

        Vector2 direction = target.position - transform.position;
        direction.Normalize();

        transform.Translate(direction * speed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, -direction);


        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 180f);
    }

    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
```


## Attack State

![AttackState](https://github.com/JeremyMarkWilcox/Unity-C-Sharp-Code-Summary/assets/150622088/b274b061-e21f-48d1-b1d4-61783f582414)



The EnemyAttackState script empowers the enemy AI with the capability to engage the player by firing projectiles when within a certain distance. This state is pivotal for adding combat dynamics to the game, making encounters with the enemy more challenging and interactive.

### Core Mechanics

**Target Acquisition:** The enemy locks onto a target, typically the player, to determine the direction and timing of attacks.

**Projectile Firing:** Upon meeting certain conditions, such as being within a specified range (maxFireDistance) and observing a cooldown period (shootInterval), the enemy fires a projectile towards the target.

**Aiming and Cooldown Management:** The script incorporates an aiming mechanism, simulating a brief aiming period (aimingTime) before each shot, adding realism to the enemy's attack pattern. The lastShootTime tracks the time since the last shot was fired to enforce shooting intervals.

### Key Features

**Distance Check:** The enemy AI checks the distance to the target before initiating an attack, ensuring that attacks are only made when the player is within effective range.

**Projectile Instantiation:** The script creates a projectile instance from a prefab (projectilePrefab) at the firePoint location, with the direction and speed set towards the target.

**Sound Effects:** An audio cue is played with each shot, enhancing the feedback and immersion of combat interactions.

**Projectile Lifespan:** Projectiles are automatically destroyed after a certain duration (projectileDuration), preventing overaccumulation in the game scene.

### Implementation Details

The shooting direction is calculated based on the target's position relative to the fire point, ensuring that projectiles are aimed accurately.
Projectile rotation is adjusted to align with the shooting direction, providing a more visually coherent attack.
The script meticulously manages the timing between shots, incorporating an aiming delay to simulate preparation time for each attack, making the enemy's behavior more predictable and allowing players to strategize.

```
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
```

## Death State

![DeathState](https://github.com/JeremyMarkWilcox/Unity-C-Sharp-Code-Summary/assets/150622088/36d381d2-8ed3-490f-8700-3b71ad59d330)

The Death state, as encapsulated within the StartDeath, Explode, and DestroyGameObject methods of the enemy AI system, signifies the final phase in the enemy's lifecycle. This state is triggered when the enemy's health reaches zero, leading to a sequence of events culminating in the enemy's removal from the game. It is included in the AI Script because it is very brief. The main reason I created the state was because I realized all of the code I had been writing to prevent the other states from triggering such as attacking. But gives time for the particle explosion and escape pod event to take place with no activity from the enemy so it can die in peace.

```
public void StartDeath()
{
    currentState = State.Death;
    Explode();
}
```

```
async void Explode()
{
    isExploding = true;
    await Task.Delay(250);
    DestroyGameObject();
}
```

## Additional Mentions

### How I implemented the enemy taking damage:
```
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
```
### How I implemented the enemy dropping an item:

```
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
```
### How I implemented enemy movement:

```
 public void Move(Vector2 direction)
 {
     direction.Normalize();
     rb.velocity = direction * speed;

     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

     angle -= -90f;

     transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
 }
```

## Boss Battle

![BossBatle](https://github.com/JeremyMarkWilcox/Unity-C-Sharp-Code-Summary/assets/150622088/b7c87429-b9ec-48b9-9e4a-c190b7a45983)


My second significant achievement in this project was developing the boss battle, an encounter that not only tests the player's skills but also enriches the gaming experience with its complexity and strategic depth.

**Dynamic Enemy Spawning:** The boss summons 8 blue enemies, reminiscent of the previous level's foes but with extended range and adjusted game logic to evoke the classic Space Invaders feel.

### Strategic Environmental Challenges

**Adaptive Barriers:** Periodically, invisible barriers activate or deactivate, methodically narrowing the space between the minions and the player. This mechanic forces players to make tactical decisions: focus on whittling down the boss's health or clear the swarm of encroaching minions.

### Boss Mechanics

**Unpredictable Movement:** The boss shares its movement script with meteorites, resulting in erratic and unpredictable floating patterns. This randomness is a deliberate design choice to balance the gameplay, given the minions' targeted aggression.

**Deadly Laser Attack:** Highlighted by a brief orange flash, the boss's capability to unleash a powerful laser beam introduces critical moments of heightened tension, requiring quick reflexes to avoid devastating damage.

### Climactic Conclusion

**Cathartic Victory:** Defeating the boss triggers the simultaneous destruction of all remaining enemies, amplifying the sense of triumph and dramatically enhancing the game's atmosphere.
This boss battle stands as a testament to the project's success, incorporating elements that challenge the player's strategic thinking and dexterity while delivering a memorable and rewarding climax.

```
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
```

## Player Ship

![PlayerMovement](https://github.com/JeremyMarkWilcox/Unity-C-Sharp-Code-Summary/assets/150622088/c7f12917-8cd9-45d2-a976-d01fde95cef0)

### Key Features of JW_HeroShip Movement:

**Dynamic Movement Control:** Players can control the ship's rotation and forward/backward movement using the horizontal and vertical input axes, respectively. The script allows for a smooth transition between movements, ensuring a responsive gaming experience.

**Speed and Rotation Limits:** It enforces maximum limits on both the ship's speed and rotation speed to prevent excessively fast movements that could make the game unplayable or disorienting.

**Visual Effects for Movement:** The script dynamically enables or disables flame game objects based on the ship's movement. Forward movement activates flames at the rear of the ship, while rotation triggers flames on the corresponding side wings, enhancing the visual feedback and immersion for the player.

**Deceleration Mechanics:** When the player stops inputting movement commands, the ship gradually slows down rather than stopping abruptly, thanks to the linear and rotational deceleration parameters. This feature adds a layer of realism to the ship's handling.

**Audio Feedback:** An AudioSource component plays a booster jet sound effect when the ship is moving, further enriching the player's interaction with the game by providing auditory cues linked to the ship's movement.

**Modular and Customizable:** The script is designed with customization in mind, featuring serialized fields for easy adjustments to speed, rotation, and deceleration values directly from the Unity Editor. This makes it versatile for various game types and design preferences.

```
using UnityEngine;

public class JW_HeroShip : MonoBehaviour
{
    private GameObject forwardFlameRightGameObject;
    private GameObject forwardFlameLeftGameObject;
    private GameObject leftWingFlameGameObject;
    private GameObject rightWingFlameGameObject;
    private GameObject BigFlameLeftGameObject;
    private GameObject backwardFlameRightGameObject;
    private GameObject backwardFlameLeftGameObject;

    [SerializeField]
    private float rotationSpeed = 100f;

    [SerializeField]
    private float maxRotationSpeed = 200f;

    [SerializeField]
    private float movementSpeed = 5f;

    [SerializeField]
    private float maxSpeed = 10f;

    [SerializeField]
    private float deceleration = 2f;

    [SerializeField]
    private float rotationDeceleration = 2f;

    private Rigidbody2D rb;
    private JW_HeroShipCollisions shipCollisions;
    public AudioSource boosterJetSound;
    private bool isMoving = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shipCollisions = GetComponent<JW_HeroShipCollisions>();    
        forwardFlameRightGameObject = transform.Find("FlameSmallRightBooster").gameObject;
        forwardFlameLeftGameObject = transform.Find("FlameSmallLeftBooster").gameObject;
        leftWingFlameGameObject = transform.Find("FlameSmallLeftWing").gameObject;
        rightWingFlameGameObject = transform.Find("FlameSmallRightWing").gameObject;
        BigFlameLeftGameObject = transform.Find("flame-big").gameObject;
        backwardFlameRightGameObject = transform.Find("RearRightBooster").gameObject;
        backwardFlameLeftGameObject = transform.Find("RearLeftBooster").gameObject;
    }

    private void Update()
    {
        float rotationInput = Input.GetAxis("Horizontal");
        float rotationAmount = -rotationInput * rotationSpeed * Time.deltaTime;

        float clampedRotationAmount = Mathf.Clamp(rotationAmount, -maxRotationSpeed * Time.deltaTime, maxRotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward * clampedRotationAmount);

        float moveInput = Input.GetAxis("Vertical");
        rb.AddForce(transform.up * moveInput * movementSpeed);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        if (moveInput == 0 && rb.velocity.magnitude > 0)
        {
            rb.AddForce(-rb.velocity.normalized * deceleration, ForceMode2D.Force);
        }

        if (rotationInput == 0 && rb.angularVelocity != 0)
        {
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0f, rotationDeceleration * Time.deltaTime);
        }

        if (moveInput > 0)
        {
            forwardFlameRightGameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            forwardFlameRightGameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (moveInput > 0)
        {
            forwardFlameLeftGameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            forwardFlameLeftGameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (moveInput > 0)
        {
            BigFlameLeftGameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            BigFlameLeftGameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        leftWingFlameGameObject.SetActive(rotationInput > 0);

        rightWingFlameGameObject.SetActive(rotationInput < 0);

        backwardFlameRightGameObject.SetActive(moveInput < 0);

        backwardFlameLeftGameObject.SetActive(moveInput < 0);

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (!isMoving)
            {
                boosterJetSound.Play();
                isMoving = true;
            }
        }
        else
        {
            if (isMoving)
            {
                boosterJetSound.Stop();
                isMoving = false;
            }
        }
    }
}
```
### Key Features of JW_HeroShipCollisions:

![Collision](https://github.com/JeremyMarkWilcox/Unity-C-Sharp-Code-Summary/assets/150622088/9b6ffd5d-a664-4bd2-bd37-0631e67075f3)


**Collision Detection:** Utilizes Unity's OnTriggerEnter2D method to detect collisions with objects tagged as "Destroyable," "Wall," "Enemy," or "Boundary." Each type of collision is handled differently, affecting the ship's lives or providing player feedback.

**Damage Management:** The ship takes damage based on specific conditions, such as colliding with an enemy or hitting a wall with sufficient velocity. The script includes a minimum velocity check to ensure that damage is only taken from significant impacts, adding a layer of realism to the collision effects.

**Lives System:** The ship starts with a predefined number of lives (maxHits), which are decremented upon taking damage. The remaining lives are displayed to the player via a TextMeshProUGUI element, providing clear feedback on the ship's status.

**Damage Cooldown:** Implements a cooldown period (damageCooldown) after taking damage during which the ship is temporarily invulnerable. This prevents rapid successive damage from quickly depleting the ship's lives.

**Visual and Audio Feedback:** Integrates with Unity's Animator and AudioSource components to play animations and sound effects corresponding to taking damage or exploding, enhancing the player's immersive experience.

**Game Over Handling:** Triggers a game over sequence by playing an explosion sound, deactivating child game objects to visually represent destruction, and loading a "Game Over" scene after a brief delay, marking the end of the game session.

**Boundary Feedback:** Provides visual feedback when the ship attempts to leave the designated play area by displaying a warning message for a short period.

**Modularity and Customization:** The script is designed to be flexible and easily integrated into various game projects. It allows for easy adjustments of key parameters like damage cooldown, minimum damage velocity, and maximum hits through serialized fields in the Unity Editor.

```
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
```


### JW_HeroShip_Projectiles Key Features:

![Projectiles](https://github.com/JeremyMarkWilcox/Unity-C-Sharp-Code-Summary/assets/150622088/a93224c9-f2ee-462c-bc5c-23b2a27e2346)

**Projectile Firing:** Enables the spaceship to fire projectiles. When the player presses the space key, a projectile is instantiated at the spaceship's position, moving in the direction the ship is facing.

**Projectile Properties:** The script defines several key properties for the projectiles, including their speed (projectileSpeed), the lifespan (projectileDuration), and the prefab to be used (projectilePrefab). These properties allow for easy adjustments to fit different game designs.

**Fire Rate Control:** Incorporates a firing rate (fireRate) mechanism to prevent continuous shooting and ensure gameplay balance. The nextFireTime variable tracks when the next shot can be fired, creating a delay between shots.

**Audio Feedback:** Integrates with an AudioSource (laserSound) to play a sound effect whenever a projectile is fired, enhancing the player's experience by providing immediate auditory feedback on their actions.

**Projectile Movement:** Applies a force to the projectile's Rigidbody2D component in the direction the ship is facing, propelling the projectile forward at the specified speed using an impulse force. This creates a dynamic and realistic projectile movement.

**Automatic Destruction:** Automatically destroys the projectile after a set duration (projectileDuration), ensuring that the game environment is not cluttered with indefinitely persisting projectiles. This is important for performance optimization and gameplay balance.

```
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
```

### There are more in the code folders!





