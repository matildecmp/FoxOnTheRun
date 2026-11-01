using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rb;
    private Animator animator;
    private Renderer renderer;
    public GameObject player;
    public GameObject mainCam;
    public GameOver gameManager;

    [Header("Movement")]
    public float playerSpeed = 8f;
    public float horizontalSpeed = 10f;
    public float rightLimit = 4f;
    public float leftLimit = -4f;
    public float jumpForce = 55f;
    public float gravityModifier = 2f;
    public float speedIncreaseRate = 0.05f; 
    public float maxSpeed = 15f;

    [Header("State")]
    public bool isGrounded = true;
    public bool isAlive = true;
    public bool isPowered = false;
    private Coroutine invincibilityCoroutine;
    public float powerUpTimer = 10f; 
    public GameObject goldenParticles;
    private GameObject activeGoldenParticles;
    public GameObject powerUpUI;
    public Slider powerUpBar;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        renderer = GetComponentInChildren<Renderer>();
        gameManager = FindObjectOfType<GameOver>();

        Physics.gravity = new Vector3(0, -9.81f, 0);
        Physics.gravity *= gravityModifier;
    }

    void Update()
    {
        if (!isAlive) return;

        // Increase Player's speed gradually
        if (playerSpeed < maxSpeed)
        {
            playerSpeed += speedIncreaseRate * Time.deltaTime;
        }

        // Handle Player's movement and jump
        PlayerMovement();
        PlayerJump();
    }
        
    // Left or right movement    
    void PlayerMovement()
    {
        float horizontalInput = 0f;

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && transform.position.x > leftLimit)
        {
            horizontalInput = -1f;
        } else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && transform.position.x < rightLimit)
        {
            horizontalInput = 1f;
        }
            
        Vector3 newVelocity = rb.linearVelocity;
        newVelocity.x = horizontalInput * horizontalSpeed;
        newVelocity.z = playerSpeed;
        rb.linearVelocity = newVelocity;
    }

    // Jump
    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetTrigger("Jump_trig");
        }
    }

    // Check if Player touches ground
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // If Player collides with Mushroom or Enemy, subtract 1 life
    void OnTriggerEnter(Collider other)
    {
        if (!isAlive || isPowered) return;

        if (other.CompareTag("Bad Object") || other.CompareTag("Enemy"))
        {
            GetComponent<HealthSystem>().TakeDamage(1);
            StartCoroutine(FlashDamage());
        }
    }

    // If Player's life count goes down to 0, game is over
    public IEnumerator PlayerDeath() // 'public' so it can be referenced in HealthSystem.cs
    {
        isAlive = false;
        
        mainCam.GetComponent<Animator>().Play("collisionCam");
        
        player.GetComponent<PlayerController>().enabled = false;
        animator.Play("Fox_Falling");

        yield return new WaitForSeconds(1);
        
        gameManager.TriggerGameOver();
    }

    // If Player collects Power Up, it won't take damage for a specified amount of time
    public void ActivateInvincibility(float timer)
    {
        // If Player is already powered up, stop and restart the coroutine
        if (isPowered && invincibilityCoroutine != null)
        {
            StopCoroutine(invincibilityCoroutine);
        }

        invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine(powerUpTimer));
    }

    // While Player is powered up, it is surrounded by a golden aura (and Power Up Bar is active)
    private IEnumerator InvincibilityCoroutine(float timer)
    {
        isPowered = true;

        if (activeGoldenParticles == null)
        {
            activeGoldenParticles = Instantiate(goldenParticles, transform);
            activeGoldenParticles.transform.localPosition = new Vector3(0, 0, 0);
            activeGoldenParticles.transform.localScale = Vector3.one * 0.3f;
        }

        powerUpUI.SetActive(true); 

        float elapsed = 0f;

        while (elapsed < timer)
        {
            elapsed += Time.deltaTime;
            if (powerUpBar != null)
                powerUpBar.value = Mathf.Clamp01(elapsed / timer);
            yield return null;
        }

        if (activeGoldenParticles != null)
        {
            Destroy(activeGoldenParticles);
            activeGoldenParticles = null;
        }

        if (powerUpUI != null)
            powerUpUI.SetActive(false); 

        isPowered = false;
    }

    // After Player takes damage, it flashes and becomes invulnerable for a certain amount of time
    public IEnumerator FlashDamage()
    {
        if (!isAlive)
        {
            yield break;
        }

        isPowered = true; 
        float flashDuration = 1f; 
        float flashInterval = 0.1f;
        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        renderer.enabled = true;
        isPowered = false;
    }
}
