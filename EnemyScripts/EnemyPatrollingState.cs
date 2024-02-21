using UnityEngine;
using System.Collections;

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
