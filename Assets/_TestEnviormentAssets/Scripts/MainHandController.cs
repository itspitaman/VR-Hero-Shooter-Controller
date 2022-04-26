using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class MainHandController : MonoBehaviour
{
    //Staff
    public Transform firePoint;
    public BoxCollider aimAssist;

    //Basic Attack 
    public GameObject gemProjectilePrefab;
    GameObject gemProjectile;
    Rigidbody gemRigidbody;

    //Special Attack
    public GameObject shieldProjectilePrefab;
    GameObject shieldProjectile;
    GameObject allyTarget;

    //Player
    public InputActionReference triggerAction;
    public InputActionReference gripAction;

    // Start is called before the first frame update
    void Start()
    {
        triggerAction.action.started += BasicAttackStarted;
        triggerAction.action.canceled += BasicAttackCanceled;
        gripAction.action.started += SpecialAttackStarted;
        gripAction.action.canceled += SpecialAttackCanceled;
    }

    private void OnDestroy()
    {
        triggerAction.action.started -= BasicAttackStarted;
        triggerAction.action.canceled -= BasicAttackCanceled;
        gripAction.action.started -= SpecialAttackStarted;
        gripAction.action.canceled -= SpecialAttackStarted;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(firePoint.position, transform.position + firePoint.forward, Color.red);
    }

    void BasicAttackStarted(InputAction.CallbackContext context)
    {
        ShootBasicAttack();
    }

    void BasicAttackCanceled(InputAction.CallbackContext context)
    {

    }

    void SpecialAttackStarted(InputAction.CallbackContext context)
    {
        if (allyTarget == null)
        {
            Debug.Log("No valid target");
        }
        else
        {
            CastShield();
        }
    }

    void SpecialAttackCanceled(InputAction.CallbackContext context)
    {

    }

    void ShootBasicAttack()
    {
        //Spawn projectile
        gemProjectile = Instantiate(gemProjectilePrefab, firePoint.transform.position, firePoint.transform.rotation, firePoint.transform);
        gemProjectile.transform.localScale = new Vector3(1f, 1f, 1f);
        gemProjectile.transform.parent = null;
    }

    void CastShield()
    {
        //Spawn shield
        shieldProjectile = Instantiate(shieldProjectilePrefab, firePoint.transform.position, firePoint.transform.rotation, firePoint.transform);
        shieldProjectile.transform.localScale = new Vector3(2f, 2f, 2f);
        shieldProjectile.transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Helllooooooooo");
        if (other.CompareTag("AllyTarget"))
        {
            allyTarget = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == allyTarget)
        {
            allyTarget = null;
        }
    }
}
