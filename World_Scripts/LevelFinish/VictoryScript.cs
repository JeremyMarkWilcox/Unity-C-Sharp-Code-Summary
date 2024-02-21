using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VictoryScript : MonoBehaviour
{
    public GameObject bossObject; 
    public AudioSource victorySoundOne;
    public AudioSource victorySoundTwo;
    public GameObject victoryPanel;
    public string menuSceneName = "JW-Title";
    private bool bossIsDead = false; 

    private void Start()
    {
        StartCoroutine(CheckBossStatus());
    }

    private IEnumerator CheckBossStatus()
    {
        yield return new WaitForSeconds(1f);

        while (!bossIsDead)
        {
            yield return new WaitForSeconds(1f); 
            if (bossObject == null)
            {
                bossIsDead = true; 
                BossDeathSequence();
            }
        }
    }

    private void BossDeathSequence()
    {
        StartCoroutine(PlayVictorySequence());
    }

    private IEnumerator PlayVictorySequence()
    {
        ShowVictoryPanel();
        yield return new WaitForSeconds(.5f);

        PlaySound(victorySoundOne);
        yield return new WaitForSeconds(1f); 

        PlaySound(victorySoundTwo);
        yield return new WaitForSeconds(2); 

        LoadMenuScene(); 
    }

    private void PlaySound(AudioSource sound)
    {
        sound?.Play();
    }

    private void ShowVictoryPanel()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }

    private void LoadMenuScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(menuSceneName);
    }
}
