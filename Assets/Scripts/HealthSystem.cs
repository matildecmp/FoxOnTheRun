using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHearts = 3;
    public int currentHealth;

    [Header("UI Elements")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private PlayerController playerController;

    // Player always starts with full health
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        currentHealth = maxHearts;
        UpdateHeartsUI();
    }

    // Update health when Player takes damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHeartsUI();

        // If health goes down to 0, Player dies
        if (currentHealth <= 0 && playerController != null && playerController.isAlive) 
        {
            StartCoroutine(playerController.PlayerDeath());
        }
    }

    // Choose heart type (full or empty) according to Player's current health 
    public void UpdateHeartsUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

        }
    }
}


