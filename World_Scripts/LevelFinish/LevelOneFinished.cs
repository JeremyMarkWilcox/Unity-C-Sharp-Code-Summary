using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOneFinished : MonoBehaviour
{
    public AudioSource levelFinishedSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            JW_HeroShipCollisions heroShipCollisions = other.GetComponent<JW_HeroShipCollisions>();
            if (heroShipCollisions != null)
            {
                levelFinishedSound.Play();
                StartCoroutine(LoadLevelWithDelay("JW-Level 2", 1.5f));
            }
        }
    }
    IEnumerator LoadLevelWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }
}