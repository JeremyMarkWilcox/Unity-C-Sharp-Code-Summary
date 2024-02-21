using System.Collections;
using UnityEngine;

public class LevelOneStart : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips; 

    void Start()
    {
        StartCoroutine(PlaySoundsSequentially());
    }

    IEnumerator PlaySoundsSequentially()
    {
        yield return new WaitForSeconds(1f);
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