using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    public float rotateSpeed = 0.5f;
    public GameObject collectSound;
    public GameObject mushParticleSystem;
    private GameObject mushParticles;

    private void Start()
    {
        // Instantiate the Mushroom's particle system
        if (CompareTag("Bad Object") && mushParticleSystem != null)
        {
            mushParticles = Instantiate(mushParticleSystem, transform.position, Quaternion.identity, transform);
        }
    }

    // Handle collectibles' behavior when Player enters their colliders
    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (other.CompareTag("Player") && playerController.isAlive)
        {
            if (CompareTag("Power Up"))
            {
                PlayCollectSound();
                other.GetComponent<Animator>().Play("Fox_Somersault_InPlace");
                playerController.ActivateInvincibility(playerController.powerUpTimer);
            }
            else if (CompareTag("Bad Object") && !playerController.isPowered)
            {
                PlayCollectSound();
            }
            else if (CompareTag("Good Object"))
            {
                PlayCollectSound();
            }

            GetComponent<Renderer>().enabled = false;
            Destroy(gameObject, 0.1f);
        }
    }

    // Play a sound when Player collects an object
    private void PlayCollectSound()
    {
        if (collectSound != null)
        {
            GameObject sound = Instantiate(collectSound, transform.position, Quaternion.identity);
            sound.GetComponent<AudioSource>().Play();
            Destroy(sound, 2f);
        }
    }

    // The collectibles spin around the Y axis continuously
    void Update()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.World);
    }
}

