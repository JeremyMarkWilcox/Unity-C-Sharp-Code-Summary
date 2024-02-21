using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffectController : MonoBehaviour
{
    public GameObject whiteFlash;

    public void TriggerDamageEffect()
    {         
        whiteFlash.SetActive(true);
        Invoke(nameof(DisableWhiteFlash), .1f);
    }

    private void DisableWhiteFlash()
    {
        whiteFlash.SetActive(false);
    }
}

