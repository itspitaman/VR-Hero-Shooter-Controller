using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public GameObject destructionParticlesPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Health collidedHealth = collision.gameObject.GetComponent<Health>();

        if(collidedHealth == null)
        {
            collidedHealth =collision.gameObject.GetComponentInParent<Health>();
        }

        if(collidedHealth == null)
        {
            collidedHealth =collision.gameObject.GetComponentInChildren<Health>();
        }

        if(collidedHealth == null)
        {
            Debug.Log("Collided with something, but there wasnt a health component.");
        }
        else
        {
            collidedHealth.TakeDamage(damage);
            Debug.Log($"{gameObject.name} dealth {damage} damage to {collidedHealth.gameObject.name}");
            Instantiate(destructionParticlesPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
