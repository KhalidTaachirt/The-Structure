using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossStateHandler : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private GameObject rockSlideObject;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private BossCondition bossCondition;
    [SerializeField] private SpawnShield spawnShield;

    private ParticleSystem rockSlide;

    public float DistanceBetweenEnemyAndPlayer { get; private set; }

    private float shockwaveTimer = 0;
    private float rockslideTimer = 0;
    private readonly float attackRange = 12f;

    private EnemyState currentState;
    private EnemyPhase currentPhase;

    private enum EnemyState
    {
        Movement,
        Melee,
        Rockslide,
        Shockwave,
    }

    private enum EnemyPhase
    {
        Start,
        Rampage,
    }

    private void Start()
    {
        rockSlide = rockSlideObject.GetComponent<ParticleSystem>();

        StartCoroutine(SpawnShieldAtInterval(10f));
    }

    private void Update()
    {
        DistanceBetweenEnemyAndPlayer = Vector3.Distance(player.position, transform.position);

        ChangeStates();
        ChangePhases();
        SetCurrentState();
        SetCurrentPhase();
    }

    // Sets the different states for every attack animation
    private void ChangeStates()
    {
        // Change the current state of the boss based on the distance 
        if (DistanceBetweenEnemyAndPlayer < attackRange)
        {
            currentState = EnemyState.Melee;
            navMeshAgent.isStopped = true;
        } 
        else 
        {
            currentState = EnemyState.Movement;
            navMeshAgent.isStopped = false;
        }
        // Exit out of the method if its still phase one
        if (currentPhase == EnemyPhase.Start)
        {
            return;
        }
        // PHASE 2
        // Start the timer for the Shockwave after the Rampage phase begins
        if (currentState == EnemyState.Movement)
        {
            ChangeStateTimer(15f, EnemyState.Shockwave);
        }
        // Start the timer for the next Rockslide after the previous Rockslide ends
        if (!rockSlide.isPlaying)
        {
            ChangeStateTimer(25f, EnemyState.Rockslide);
        }
    }

    // Changes the phases based on the current conditions of the boss
    private void ChangePhases()
    {
        if (bossCondition.BossHealth <= bossCondition.BossMaxHealth / 2 &&
		currentPhase != EnemyPhase.Rampage)
        {
            currentPhase = EnemyPhase.Rampage;
        }
    }

    // Sets the different bools during the start phase
    private void SetCurrentPhase()
    {
        switch (currentPhase)
        {
            case EnemyPhase.Start:
                // TODO: Growl Sound Effect
                break;
            case EnemyPhase.Rampage:
                bossAnimator.SetBool("isRampaged", true);
                break;
            default:
                Debug.Log("No phase found");
                break;
        }
    }

    // Sets the different bools during the rampage phase
    private void SetCurrentState()
    {
        switch (currentState)
        {
            case EnemyState.Melee:
                bossAnimator.SetBool("isAttacking", true);
                break;
            case EnemyState.Movement:
                bossAnimator.SetBool("isAttacking", false);
                break;
            case EnemyState.Rockslide:
                bossAnimator.SetBool("isRockslide", true);
                break;
            case EnemyState.Shockwave:
                bossAnimator.SetBool("isShockwave", true);
                break;
            default:
                Debug.Log("No state found");
                break;
        }
    }

    // By giving a value you assign the current state after a certain amount of time
    private void ChangeStateTimer(float repeatTime, EnemyState state)
    {
        if (state == EnemyState.Shockwave)
        {
            if (shockwaveTimer < 0)
            {
                currentState = state;
                shockwaveTimer = repeatTime;
            }

            shockwaveTimer -= Time.deltaTime;
        }

        if (state == EnemyState.Rockslide)
        {
            if (rockslideTimer < 0)
            {
                currentState = state;
                rockslideTimer = repeatTime;
            }

            rockslideTimer -= Time.deltaTime;
        }
    }

    private IEnumerator SpawnShieldAtInterval(float secondsBetweenSpawns)
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsBetweenSpawns);

            spawnShield.BossShield();
        }      
    }

    /// <summary>
    /// Allows the navmesh agent to move towards the player
    /// the parameter controls the speed of the movement
    /// </summary>
    /// <param name="speed"></param>
    public void MoveToPlayer(float speed)
    {
        navMeshAgent.destination = player.position;
        navMeshAgent.speed = speed;
    } 
}