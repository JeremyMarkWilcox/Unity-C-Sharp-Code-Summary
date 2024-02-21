using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationHelper : MonoBehaviour
{
    private GameObject targetObject;
    private float offInterval;
    private float cooldownInterval;

    public void Init(GameObject target, float off, float cooldown)
    {
        targetObject = target;
        offInterval = off;
        cooldownInterval = cooldown;
        StartCoroutine(ActivationRoutine());
    }

    IEnumerator ActivationRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldownInterval);
            if (targetObject != null)
            {
                targetObject.SetActive(false);
            }
            yield return new WaitForSeconds(offInterval);
            if (targetObject != null)
            {
                targetObject.SetActive(true);
            }
        }
    }
}