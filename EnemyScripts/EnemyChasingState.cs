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
