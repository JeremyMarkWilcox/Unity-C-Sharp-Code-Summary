using System.Collections;
using UnityEngine;

public class LevelTwoStart : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips; 

    void Start()
    {
        StartCoroutine(PlaySoundsSequentially());
    }

    IEnumerator PlaySoundsSequentially()
    {
        foreach (var clip in audioClips)
        {
            audioSource.clip = clip;
            audioSource.Play();

            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }
    }
}