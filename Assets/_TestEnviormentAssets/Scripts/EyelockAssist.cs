using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyelockAssist : MonoBehaviour
{
    BasicHealthComponent currentTarget;
    bool canTarget;
    bool canAttack;
    float cooldown;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        canTarget = true;
        canAttack = true;
        cooldown = 3f;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            canAttack = true;
            
            if (currentTarget != null)
            {
                DealSomeDamage(25);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        currentTarget = other.GetComponent<BasicHealthComponent>();

        if (canAttack)
        {
            if (currentTarget != null && currentTarget.CompareTag("Target"))
            {
                DealSomeDamage(25);
            }
        }
    }

    void DealSomeDamage(int damage)
    {
        currentTarget.DealDamage(damage);
        canAttack = false;
        timer = cooldown;
    }

    private void OnTriggerExit(Collider other)
    {
        currentTarget = null;
    }

    void IsTargetDead()
    {
        if (currentTarget == null)
        {
            canTarget = true;
        }
    }
}
