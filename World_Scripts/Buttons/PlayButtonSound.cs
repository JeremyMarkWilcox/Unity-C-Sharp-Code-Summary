using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayButtonSound : MonoBehaviour
{
    public AudioSource buttonAudioSource; 
    private string nextSceneName = "JW-Level 1";

    void Start()
    {
        DontDestroyOnLoad(buttonAudioSource.gameObject);
        GetComponent<Button>().onClick.AddListener(PlaySoundAndLoadScene);
    }

    public void PlaySoundAndLoadScene()
    {
        buttonAudioSource.Play();
        SceneManager.LoadScene(nextSceneName);
    }
}

