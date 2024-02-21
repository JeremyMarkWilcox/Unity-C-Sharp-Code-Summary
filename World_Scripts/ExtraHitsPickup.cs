using UnityEngine;

public class ExtraHitsPickup : MonoBehaviour
{
    public int extraHitsAmount = 1;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();              
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            JW_HeroShipCollisions heroShipCollisions = other.GetComponent<JW_HeroShipCollisions>();
            if (heroShipCollisions != null)
            {
                GetComponent<Collider2D>().enabled = false;
                GetComponent<Renderer>().enabled = false;
                heroShipCollisions.AddExtraHits(extraHitsAmount);          
                audioSource.Play();
                Destroy(gameObject, audioSource.clip.length);
            }
        }
    }
}
