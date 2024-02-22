# Unity/ C# Code Summary On Live Project
I created an upgraded 2D version of Space Invaders with the Unity Engine, written in C#

## Introduction

In my final two courses, I delved into game development with Unity/C# and Unreal/C++. Each course culminated in a two-week live project, where I contributed an original game to a larger collaborative effort. My practical understanding of Agile/Scrum methodologies deepened through daily stand-ups and weekly code retrospectives, which bolstered accountability and fine-tuned our code management practices. Mastery of version control was essential due to the frequent pull requests and updates our team managed daily.

Designing my own video game was an invaluable opportunity to explore Unity's game engine intricacies and harness specific C# libraries for game functionality. Debugging and iterative development were constant companions as I expanded the game's features. Among the project's milestones, I am proud to have been the first student in the Bootcamp to implement a finite state machine successfully and the second to engineer a boss battle.

Enclosed are snippets from my code contributions, with the complete codebase accessible in the accompanying files.

## Play On My Personal Website

Here is a link to my portfolio where you can play the mini game in full:


## Game Scenes

## Enemy AI/ Finite State Machine

Enemy AI Finite State Machine (FSM) Summary
The EnemyAI component implements a finite state machine (FSM) to dynamically control enemy behavior in four key states: Patrolling, Chasing, Attacking, and Death. This system allows the enemy AI to seamlessly transition between behaviors based on player proximity and interactions:

Initialization: Begins in Patrolling, roaming the area.
Player Detection: Transitions to Chasing when the player is detected within a certain range.
Close Proximity: Switches to Attacking to engage the player upon getting close enough.
Taking Damage: Handles taking damage and triggers the Death state upon health depletion, leading to an explosion effect and the enemy's destruction.
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
### Patroll State
The currentState variable plays a pivotal role in the enemy AI's decision-making process, acting as the switchboard between various behavioral states. By default, the AI begins in a Patrolling state upon the player's first encounter with it. This state represents the AI's routine behavior, where it navigates through predefined waypoints or wanders within a designated area.

As the game progresses and the player interacts with the AI, the currentState can transition to other states such as Chasing, Attacking, or Death based on the situation. For example, if the player comes within a certain proximity to the enemy, detected by the chaseTriggerDistance, the AI shifts from Patrolling to Chasing, signifying a change in behavior as it actively pursues the player.

This state-switching mechanism is facilitated by a series of conditional checks within the AI's update loop, with transitions triggered by specific game events or conditions being met. The default Patrolling state ensures that the enemy exhibits autonomous behavior, providing a dynamic and engaging experience from the outset of the encounter.

![PatrollingState](https://github.com/JeremyMarkWilcox/Unity-C-Sharp-Code-Summary/assets/150622088/c547acd4-f892-4031-8929-7baf623e33c2)

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

### Chase State

The Chasing state, as defined in the Enemy Chasing State script, orchestrates the enemy's behavior when it has detected and is actively pursuing the player. This script is a vital component of the enemy AI's ability to engage with the player in a dynamic and challenging manner.

Functionality
Target Acquisition: The script requires a target, typically the player, which is set through the SetTarget method. This allows the enemy to dynamically focus on the player regardless of their position in the game world.
Movement: Upon each frame update, the enemy calculates the direction to the target and moves towards it at a specified speed (speed), ensuring a continuous pursuit.
Distance Maintenance: While the script doesn't explicitly enforce maintaining a set distance from the player (maintainDistance), this value could be used to modify behavior, such as transitioning to an attack state or adjusting pursuit speed to avoid getting too close or too far.
Rotation: The enemy rotates to face the player as it moves, creating a more lifelike and engaging pursuit. This rotation is smoothly handled to always orient the enemy towards the player, enhancing visual fidelity.
Key Aspects
Adaptive Pursuit: The script ensures the enemy can adapt its chase in real-time to the player's movements, providing a realistic and responsive AI behavior.
Error Handling: It includes a safeguard against a missing target, which logs a warning. This feature aids in debugging and ensures the game remains robust against unexpected errors.
Efficient Targeting: Through normalization of the direction vector and the use of quaternion rotation, the script ensures efficient and smooth pursuit mechanics.








## Meteors

## Player Ship







