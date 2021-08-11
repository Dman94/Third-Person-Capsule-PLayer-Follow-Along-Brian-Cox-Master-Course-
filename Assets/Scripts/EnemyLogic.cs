using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


enum EnemyState
{
    idol,
    patrol,
    chase,
    attack
}
public class EnemyLogic : MonoBehaviour
{
    [SerializeField] EnemyState currentState = EnemyState.idol;

    Vector3 CurrentpatrolDestination;
    [SerializeField] Transform destination;
    [SerializeField] Transform PatrolStartPosition;
    [SerializeField] Transform PatrolEndPosition;

    AudioSource Audiosource;
    [SerializeField] AudioClip EnemyHit;
    [SerializeField] AudioClip BloodSplat;

    [SerializeField]float AggroRadius = 5.0f;

    NavMeshAgent agent;

    
    GameObject Player;
    Playerlogic playerLogic;








    float stoppingDistance = 2f;
    int Damage = 10;

    float MelleRadius = 2f;
    const float MaxATtackCooldown = 0.5f;
    float AttackCooldown = MaxATtackCooldown;
    int Health = 100;

    const float TimeRefill = 1f;
    float waitTime = TimeRefill;






    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        currentState = EnemyState.patrol;
        Audiosource = GetComponent<AudioSource>();
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, AggroRadius);

        Gizmos.color = new Color(10, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, MelleRadius);
    }
    
    void Update()
    {
     

        switch(currentState)
        {
           case(EnemyState.idol):
                SearchForPlayer();
                break;

            case (EnemyState.patrol):
                SearchForPlayer();
                if (PatrolStartPosition && PatrolEndPosition)
                {  
                 Patrol();
                }
                break;

            case (EnemyState.chase):
                ChasePlayer();
                break;

            case (EnemyState.attack):
                UpdateAttack();
                break;
        }

    }

    private void ChasePlayer()
    {
        if (agent && destination)
        {
            agent.SetDestination(destination.position);
        }

        float distance = Vector3.Distance(transform.position, destination.position);
        if (distance < stoppingDistance)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            currentState = EnemyState.attack;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    void Patrol()
    {
        if (agent && CurrentpatrolDestination != Vector3.zero)
        {
            agent.SetDestination(CurrentpatrolDestination);
        }

        float distance = Vector3.Distance(transform.position, CurrentpatrolDestination);

        if (distance < stoppingDistance)
        {
           if (CurrentpatrolDestination == PatrolStartPosition.position)
           {
                //wait 1 second
                waitTime -= Time.deltaTime;
                if(waitTime < 0) 
                {
                    /*then assign the next positiion*/
                    CurrentpatrolDestination = PatrolEndPosition.position;
                    waitTime = TimeRefill;
                }
                
               
           }
           else
           {
                CurrentpatrolDestination = PatrolStartPosition.position;
           }
        }

    }
    void SearchForPlayer()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        if (distance < AggroRadius)
        {
            Debug.Log("Within radius");
            currentState = EnemyState.chase;
        }
    }
    void UpdateAttack()
    {
        float Distance = Vector3.Distance(Player.transform.position, transform.position);

        if(Distance < MelleRadius)
        {
            AttackCooldown -= Time.deltaTime; // reduce the attack cooldown

            if(AttackCooldown < 0) // enemy is standing close to player for atleast half a second
            {
                //Attack the player
                playerLogic = Player.GetComponent<Playerlogic>();
                if (playerLogic)
                {
                    playerLogic.TakeDamage(Damage);
                    Audiosource.PlayOneShot(EnemyHit);
                }

                // reset attack cooldown
                AttackCooldown = MaxATtackCooldown;
            }
           
        }
        else
        {
            currentState = EnemyState.chase;
        }
    }
    
    public void TakeDamage(int Damage)
    {
        Health -= Damage;

        Audiosource.PlayOneShot(BloodSplat);

        if(Health <=0)
        {
            Destroy(gameObject);

            
        }
    }
}

