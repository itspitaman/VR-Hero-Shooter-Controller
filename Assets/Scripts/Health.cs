using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    public int currentHitPoints;
    public int maxHitPoints = 100;

    public bool isEnemy;
    public bool isCampfire;
    public bool canRespawn; //this is checked only if they are a player
    public bool isDead;

    public Slider healthSlider;

    public ActionBasedContinuousMoveProvider moveProvider;
    public float originalMoveSpeed;

    public GameObject respawnCanvas;
    public Text respawnTimerText;
    public float respawnTimer;
    float timeSpentRespawning;

    public Transform playerTransform;
    public Transform respawnTransform;

    public InputActionReference[] respawnAction;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.maxValue = maxHitPoints;
        healthSlider.value = currentHitPoints;

        if(canRespawn)
        {
            moveProvider.moveSpeed = originalMoveSpeed;
            respawnCanvas.SetActive(false);

            foreach(InputActionReference reference in respawnAction)
            {
                reference.action.performed += TriggerRespawn;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthSlider()
    {
        healthSlider.value = currentHitPoints;
    }

    public void TakeDamage(int damageTaken)
    {
        currentHitPoints = Mathf.Max(0, currentHitPoints - damageTaken);
        Debug.Log($"{gameObject.name} took {damageTaken} damage and is now at {currentHitPoints}");
        UpdateHealthSlider();

        if(currentHitPoints <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;

        if(isEnemy)
        {
            Debug.Log("Enemy down!");
            Destroy(gameObject);
        }

        if(canRespawn)
        {
            moveProvider.moveSpeed = 0;
            StartCoroutine(RespawnTimer());
        }
    }

    IEnumerator RespawnTimer()
    {
        respawnCanvas.SetActive(true);
        timeSpentRespawning = 0f;

        while(timeSpentRespawning < respawnTimer)
        {
            timeSpentRespawning += Time.deltaTime;
            currentHitPoints = Mathf.RoundToInt(timeSpentRespawning / respawnTimer * maxHitPoints);
            UpdateHealthSlider();
            respawnTimerText.text = $"Respawning in: {Mathf.RoundToInt(respawnTimer - timeSpentRespawning)}";
            yield return null;
        }

        Respawn();
    }

    void Respawn()
    {
        if(isDead)
        {
            StopAllCoroutines();
            respawnCanvas.SetActive(false);
            moveProvider.moveSpeed = originalMoveSpeed;
            isDead = false;
            playerTransform.position = respawnTransform.position;
            playerTransform.rotation = respawnTransform.rotation;
        }
    }

    void TriggerRespawn(InputAction.CallbackContext context)
    {
        Respawn();
    }

    private void OnDestroy()
    {
        if(canRespawn)
        {
            foreach (InputActionReference reference in respawnAction)
            {
                reference.action.performed -= TriggerRespawn;
            }
        }
    }
}
