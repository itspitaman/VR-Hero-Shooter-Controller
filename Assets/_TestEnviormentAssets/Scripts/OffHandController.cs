using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class OffHandController : MonoBehaviour
{
    // Player
    public InputActionReference triggerAction;
    public InputActionReference gripAction;

    // Dash
    public GameObject locomotionSystem;
    ActionBasedContinuousMoveProvider playerMovement;

    public float dashSpeed;
    public float dashCooldown;
    public float dashDuration;
    public Text dashText;

    float normalSpeed;
    bool canDash;
    float cooldownTimer;

    // Inspector variables
    public bool activateDash;

    // Start is called before the first frame update
    void Start()
    {
        triggerAction.action.started += BasicAbilityStarted; //need to change the names of abiltities before adding code
        triggerAction.action.canceled += BasicAbilityCanceled;
        gripAction.action.started += SpecialAbilityStarted;
        gripAction.action.canceled += SpecialAbilityCanceled;

        playerMovement = locomotionSystem.GetComponent<ActionBasedContinuousMoveProvider>();
        normalSpeed = playerMovement.moveSpeed;
        canDash = true;
        cooldownTimer = 0f;

        UpdateDashUI();
    }

    private void OnDestroy()
    {
        triggerAction.action.started -= BasicAbilityStarted;
        triggerAction.action.canceled -= BasicAbilityCanceled;
        gripAction.action.started -= SpecialAbilityStarted;
        gripAction.action.canceled -= SpecialAbilityStarted;
    }

    // Update is called once per frame
    void Update()
    {
        if(activateDash && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void BasicAbilityStarted(InputAction.CallbackContext context)
    {

    }

    void BasicAbilityCanceled(InputAction.CallbackContext context)
    {

    }

    void SpecialAbilityStarted(InputAction.CallbackContext context)
    {
        if(canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void SpecialAbilityCanceled(InputAction.CallbackContext context)
    {

    }

    IEnumerator Dash()
    {
        playerMovement.moveSpeed = dashSpeed;
        canDash = false;

        yield return new WaitForSeconds(dashDuration);

        playerMovement.moveSpeed = normalSpeed;
        StartCooldownTimer(dashCooldown);

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    void UpdateDashUI()
    {
        dashText.text = ($"Dash: {cooldownTimer}");
    }

    void StartCooldownTimer(float cd)
    {
        cooldownTimer = cd;

        while (cooldownTimer > 0)
        {
            UpdateDashUI();
            cooldownTimer -= Time.deltaTime;
        }
    }
}
