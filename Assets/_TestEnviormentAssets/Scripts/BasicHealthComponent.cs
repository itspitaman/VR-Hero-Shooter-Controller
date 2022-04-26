using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicHealthComponent : MonoBehaviour
{
    public Slider healthBar;

    public int maxHealth = 100;
    public int currentHealth;

    public bool isEnemy;
    public bool isPlayer;
    public bool isAlly;
    public bool isTarget;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DealDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        UpdateHealthBar();
        Debug.Log($"{gameObject.name} took {damage} damage. Current Health: {currentHealth}");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void Heal(int healing)
    {
        currentHealth = Mathf.Min(100, currentHealth + healing);
        UpdateHealthBar();
        Debug.Log($"{gameObject.name} recieved {healing} healing. Current Health: {currentHealth}");
    }

    public void UpdateHealthBar()
    {
        healthBar.value = currentHealth;
    }
}
