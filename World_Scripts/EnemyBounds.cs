using UnityEngine;

public class EnemyBounds : MonoBehaviour
{
    public float offInterval = 0.5f;
    public float cooldownInterval = 5f;

    private void Start()
    {
        new GameObject("HelperFor_" + gameObject.name).AddComponent<ActivationHelper>().Init(gameObject, offInterval, cooldownInterval);
    }

    public void DeactivateTemporarily()
    {
        gameObject.SetActive(false);
    }
}
