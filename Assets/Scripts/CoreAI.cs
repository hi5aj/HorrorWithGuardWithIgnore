using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CoreAI : MonoBehaviour
{
    private enum AIState
    {
        Passive,
        Hostile,
        Stunned,
        Angry
    }

    private AIState _AIState;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;
    public bool isChasingPlayer;
    public bool IAmWaiting;
    public bool randomWaitTime;
    public bool randomWander;
    public bool alwaysMoving;
    public bool fleeFromPlayer;
    public bool isStunned;

    [SerializeField]
    [Range(1, 7)] private int wait_time;

    [SerializeField]
    private float speed;
    private float timeSinceSeenPlayer;

    [SerializeField]
    [Range(0, 500)] private float walkRadius;

    public float FoVRadius;
    [Range(0, 360)] public float FoVAngle;

    public float proximityRadius;
    [Range(0, 360)] public float proximityAngle;

    [SerializeField]
    private Transform[] waypoints;

    private int nextWayPoint = 0;

    [HideInInspector]
    public GameObject player;

    public Renderer enemyColor;

    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        enemyColor = GetComponent<Renderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch (_AIState)
        {
            case AIState.Passive:
                enemyColor.material.color = Color.yellow;
                if (randomWander == true)
                {
                    Wander();
                    if (canSeePlayer == true)
                    {
                        _AIState = AIState.Hostile;
                    }
                }
                else
                {
                    if (navMeshAgent.remainingDistance < 2f && canSeePlayer == false)
                    {
                        GotoNextPoint();
                    }
                    if (canSeePlayer == true)
                    {
                        _AIState = AIState.Hostile;
                    }
                }
                break;

            case AIState.Hostile:
                ChasePlayer();
                navMeshAgent.speed = 10;
                if (canSeePlayer == false)
                {
                    FieldOfViewCheck();
                }
                break;
        }
        ProximityCheck();
    }

    IEnumerator RandomWaitTimer()
    {
        if (alwaysMoving == false && randomWaitTime == true)
        {
            wait_time = Random.Range(1, 5);
            navMeshAgent.speed = 0;
            yield return new WaitForSeconds(wait_time);
        }
        else if (alwaysMoving == false && randomWaitTime == false)
        {
            navMeshAgent.speed = 0;
            yield return new WaitForSeconds(wait_time);
        }
        navMeshAgent.speed = speed;
        IAmWaiting = false;
    }

    public Vector3 RandomNavMeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere * walkRadius;
        randomPosition += transform.position;
        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    private void Wander()
    {
        if (navMeshAgent != null && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && IAmWaiting == false)
        {
            navMeshAgent.SetDestination(RandomNavMeshLocation());
            IAmWaiting = true;
            StartCoroutine(RandomWaitTimer());
        }
    }
    
    void GotoNextPoint()
    {
        if (waypoints.Length == 0)
            return;
        navMeshAgent.destination = waypoints[nextWayPoint].position;
        nextWayPoint = (nextWayPoint + 1) % waypoints.Length;
        IAmWaiting = true;
        StartCoroutine(RandomWaitTimer());
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, FoVRadius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < FoVAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }

        }
        else if (canSeePlayer == false)
        {
            canSeePlayer = false;
            timeSinceSeenPlayer += Time.deltaTime;

            if (timeSinceSeenPlayer >= 2f)
            {
                canSeePlayer = false;
                isChasingPlayer = false;
                _AIState = AIState.Passive;
                timeSinceSeenPlayer = 0;
            }
        }

    }

    private void ProximityCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, proximityRadius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < proximityAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer == false)
        {
            canSeePlayer = false;
            timeSinceSeenPlayer += Time.deltaTime;

            if (timeSinceSeenPlayer >= 2f)
            {
                canSeePlayer = false;
                isChasingPlayer = false;
                _AIState = AIState.Passive;
                timeSinceSeenPlayer = 0;
            }
        }
    }

    private IEnumerator CheckForPlayer()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void ChasePlayer()
    {
        isChasingPlayer = true;
        navMeshAgent.destination = player.transform.position;
        if (canSeePlayer == true)
        {
            enemyColor.material.color = Color.red;
        }
        else
        {
            enemyColor.material.color = Color.magenta;
        }
        FieldOfViewCheck();
    }
}
