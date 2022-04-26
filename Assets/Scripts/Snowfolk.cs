using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Snowfolk : MonoBehaviour
{
    public enum SnowfolkState { Wander, Chase, Attack};
    public SnowfolkState currentState;

    FieldOfView fov;
    public Health currentTarget;
    NavMeshAgent navigationAgent;

    float chaseTimer;
    public float chaseGiveUpTimer;
    public float wanderSpeed;
    public float chaseSpeed;
    public float attackRange;
    public float attackSpeed;
    float attackTimer;
    public float snowBallArcHeight = 4f;

    public GameObject snowBallPrefab;
    public Transform snowBallSpawnPoint;
    GameObject spawnedSnowBall;
    Rigidbody snowBallRigidbody;
    Projectile snowBallProjectile; //The snow ball that actually gets thrown when held

    // Start is called before the first frame update
    void Start()
    {
        currentState = SnowfolkState.Wander;
        currentTarget = null; 
        fov = GetComponentInChildren<FieldOfView>();
        navigationAgent = GetComponent<NavMeshAgent>();
        attackTimer = 0f;
        SpawnSnowBall();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == SnowfolkState.Wander)
        {
            Wander();
        }
        else if(currentState == SnowfolkState.Chase)
        {
            Chase();
        }
        else if(currentState == SnowfolkState.Attack)
        {
            Attack();
        }
    } 

    bool CanSeeAttackableHealth()
    {
        if(fov.visibleHealths.Count > 0)
        {
            foreach(Health seenHealth in fov.visibleHealths)
            {
                if(!seenHealth.isEnemy && !seenHealth.isDead)
                {
                    currentTarget = seenHealth;
                    currentState = SnowfolkState.Chase;
                    return true;
                }
            }
        }

        return false;
    }

    void Wander()
    {
        currentTarget = null;

        if (CanSeeAttackableHealth())
        {
            return;
        }
        
        navigationAgent.speed = wanderSpeed;

        if(navigationAgent.remainingDistance <= navigationAgent.stoppingDistance) // added isdead check
        {
            Vector3 randomDirection = transform.position + Random.insideUnitSphere * 5;
            NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 5, 1);
            navigationAgent.SetDestination(hit.position);
        }
    }

    void Chase()
    {
        if(CanSeeAttackableHealth())
        {
            chaseTimer = chaseGiveUpTimer;
        }
        else
        {
            chaseTimer -= Time.deltaTime;

            if(chaseTimer <= 0)
            {
                currentState = SnowfolkState.Wander;
                return;
            }
        }

        if(navigationAgent.remainingDistance <= attackRange)
        {
            currentState = SnowfolkState.Attack;
            return;
        }

        navigationAgent.SetDestination(currentTarget.transform.position);
        navigationAgent.speed = chaseSpeed;
    }

    void Attack()
    {
        var distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

        if(distanceToTarget > attackRange)
        {
            navigationAgent.SetDestination(currentTarget.transform.position);
            currentState = SnowfolkState.Chase;
            return;
        }

        navigationAgent.SetDestination(transform.position); // added currentTarget
        transform.LookAt(new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z));

        if(currentTarget.isDead || currentTarget == null)
        {
            currentState = SnowfolkState.Wander;

            return;
        }

        //Here is where we can attack the enemy

        attackTimer -= Time.deltaTime;
        if(attackTimer <= 0f)
        {
            ThrowSnowBall();
            attackTimer = attackSpeed;
        }
    }

    void SpawnSnowBall()
    {
        spawnedSnowBall = Instantiate(snowBallPrefab, snowBallSpawnPoint.transform.position, snowBallSpawnPoint.transform.rotation, snowBallSpawnPoint.transform);
        snowBallProjectile = spawnedSnowBall.GetComponent<Projectile>();
        snowBallRigidbody = spawnedSnowBall.GetComponent<Rigidbody>();
        snowBallRigidbody.useGravity = false;
        snowBallRigidbody.isKinematic = true;
        spawnedSnowBall.GetComponent<SphereCollider>().enabled = false;
    }

    void ThrowSnowBall()
    {
        spawnedSnowBall.transform.parent = null;
        snowBallRigidbody.useGravity = true;
        snowBallRigidbody.isKinematic = false;
        snowBallProjectile.GetComponent<SphereCollider>().enabled = true;
        snowBallRigidbody.AddForce(CalculateLaunchVelocity(), ForceMode.VelocityChange);

        StartCoroutine(SpawnSnowBallAfterSeconds());
        StartCoroutine(DestroyAfterSeconds(snowBallProjectile.gameObject));
    }

    Vector3 CalculateLaunchVelocity()
    {
        float displacementY = currentTarget.transform.position.y - snowBallRigidbody.position.y;
        Vector3 displacementXZ = new Vector3(currentTarget.transform.position.x - snowBallRigidbody.position.x, 0, currentTarget.transform.position.z - snowBallRigidbody.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * snowBallArcHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * snowBallArcHeight / Physics.gravity.y) +
                                               Mathf.Sqrt(2 * (displacementY - snowBallArcHeight) / Physics.gravity.y));

        return velocityXZ + velocityY;
    }

    IEnumerator SpawnSnowBallAfterSeconds()
    {
        yield return new WaitForSeconds(attackSpeed / 2);
        SpawnSnowBall();
    }

    IEnumerator DestroyAfterSeconds(GameObject destroyThis)
    {
        yield return new WaitForSeconds(6f);
        
        if(destroyThis != null)
        {
            Destroy(destroyThis);
        }
    }
}
