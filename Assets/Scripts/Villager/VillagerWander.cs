using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class VillagerWander : MonoBehaviour
{
    public float wanderSpeed = 0.5f; // Speed for normal wandering
    public float waypointReachDistance = 0.5f; // Distance threshold to consider villager at target
    public float idleTimeAtWaypoint = 2f; // Time to idle at each waypoint before choosing a new one

    private AIPath aiPath;
    private Vector3 targetPosition;
    private bool isWalking = false;

    private static List<Transform> wanderPoints = new List<Transform>();

    void Awake()
    {
        aiPath = GetComponent<AIPath>();

        // Populate wanderPoints if not already populated
        if (wanderPoints.Count == 0)
        {
            foreach (GameObject point in GameObject.FindGameObjectsWithTag("Wanderpoint"))
            {
                wanderPoints.Add(point.transform);
            }
        }
    }

    void OnEnable()
    {
        // Set the AIPath speed to the wander speed whenever this script is enabled
        aiPath.maxSpeed = wanderSpeed;
    }

    void OnDisable()
    {
        aiPath.destination = transform.position; // Set destination to current position
        aiPath.maxSpeed = 0f;                    // Stop movement
        //aiPath.enabled = false;                  // Optionally, disable AIPath to stop recalculations
        isWalking = false;
        enabled = false;                         // Disable this fleeing script
    }

    void Start()
    {
        StartCoroutine(WanderRoutine());
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            if (!isWalking)
            {
                ChooseRandomWaypoint();
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void ChooseRandomWaypoint()
    {
        if (wanderPoints.Count == 0)
        {
            Debug.LogWarning("No wander points available!");
            return;
        }

        // Select a random waypoint
        int randomIndex = Random.Range(0, wanderPoints.Count);
        Transform chosenWaypoint = wanderPoints[randomIndex];

        // Get the Collider2D center of the waypoint (if it has one)
        Collider2D waypointCollider = chosenWaypoint.GetComponent<Collider2D>();
        targetPosition = waypointCollider != null ? waypointCollider.bounds.center : chosenWaypoint.position;

        // Set the destination for A* pathfinding
        aiPath.destination = targetPosition;
        aiPath.SearchPath();

        isWalking = true;
    }

    void Update()
    {
        // Check if the villager has reached the waypoint
        if (!aiPath.pathPending && aiPath.reachedEndOfPath && Vector3.Distance(transform.position, targetPosition) <= waypointReachDistance)
        {
            StartCoroutine(IdleAtWaypoint());
        }
    }

    private IEnumerator IdleAtWaypoint()
    {
        isWalking = false;
        
        // Wait at the waypoint for a short duration before choosing a new target
        yield return new WaitForSeconds(idleTimeAtWaypoint);

        // Choose a new random waypoint after idling
        ChooseRandomWaypoint();
    }
}
