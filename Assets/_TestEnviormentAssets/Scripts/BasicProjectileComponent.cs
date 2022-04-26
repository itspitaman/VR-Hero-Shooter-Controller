using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectileComponent : MonoBehaviour
{
    public float projectileSpeed;

    public bool canDamage;
    public bool canHeal;

    public int damage;
    public int healing;

    //Homing misile
    public bool isHoming;
    public float launchSpeed;
    public float rotationSpeed;
    Transform homingTarget;
    Vector3 direction;
    Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        if (isHoming)
        {
            homingTarget = GameObject.Find("TargetAlly").transform;

            //Launch upward force
            rb.AddForce(Vector3.up * launchSpeed);
        }
        else
        {
            rb.AddRelativeForce(Vector3.forward * projectileSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (isHoming)
        {
            //Movement
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);

            //Rotation
            if (homingTarget != null)
            {
                direction = homingTarget.position - transform.position;
                direction = direction.normalized;

                var rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        BasicHealthComponent targetHit = collision.gameObject.GetComponent<BasicHealthComponent>();

        if (targetHit == null)
        {
            targetHit = collision.gameObject.GetComponentInParent<BasicHealthComponent>();
        }

        if (targetHit == null)
        {
            targetHit = collision.gameObject.GetComponentInChildren<BasicHealthComponent>();
        }

        if (targetHit == null)
        {
            Debug.Log("Target hit does not have a Health Component.");
        }
        else
        {
            if (canDamage)
            {
                targetHit.DealDamage(damage);
                Debug.Log($"{gameObject.name} dealt {damage} damage to {targetHit.gameObject.name}");
            }

            else if (canHeal)
            {
                targetHit.Heal(healing);
                Debug.Log($"{gameObject.name} healed {damage} to {targetHit.gameObject.name}");
            }
                
            Destroy(gameObject);
        }
    }
}
