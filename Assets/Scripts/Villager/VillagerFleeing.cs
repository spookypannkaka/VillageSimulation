using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; // Import A* Pathfinding namespace

public class VillagerFleeing : MonoBehaviour
{
    public float fleeSpeed = 20f;
    private Vector3 targetPosition;
    private AIPath aiPath;

    private static List<Transform> entranceSpots = new List<Transform>();
    private static List<Transform> availableSpots;

    void Awake()
    {
        aiPath = GetComponent<AIPath>();

        if (entranceSpots.Count == 0)
        {
            foreach (GameObject building in GameObject.FindGameObjectsWithTag("Building"))
            {
                Transform entranceSpot = building.transform.Find("EntranceSpot");
                if (entranceSpot != null)
                {
                    entranceSpots.Add(entranceSpot);
                }
            }
        }

        if (availableSpots == null || availableSpots.Count == 0)
        {
            availableSpots = new List<Transform>(entranceSpots);
        }
    }

    void OnEnable()
    {
        if (availableSpots.Count == 0)
        {
            Debug.LogWarning("No entrance spots available!");
            return;
        }

        aiPath.enabled = true;
        aiPath.maxSpeed = fleeSpeed;
        aiPath.canMove = true;

        // Choose a random EntranceSpot and get its collider's center
        int randomIndex = Random.Range(0, availableSpots.Count);
        Transform chosenSpot = availableSpots[randomIndex];
        //availableSpots.RemoveAt(randomIndex);

        // Get the Collider2D component from the chosen EntranceSpot
        Collider2D entranceCollider = chosenSpot.GetComponent<Collider2D>();
        if (entranceCollider != null)
        {
            // Set the targetPosition to the center of the collider
            targetPosition = entranceCollider.bounds.center;
        }
        else
        {
            Debug.LogWarning("No Collider2D found on EntranceSpot. Using Transform position as fallback.");
            targetPosition = chosenSpot.position; // Fallback in case there's no collider
        }

        // Set the target position for A* pathfinding
        aiPath.destination = targetPosition;
        aiPath.SearchPath(); // Force recalculation of the path
    }

    void Update()
    {
        // Check if the villager has reached the entrance spot
        if (!aiPath.pathPending && aiPath.reachedEndOfPath)
        {
            DespawnVillager();
        }
    }

    private void DespawnVillager()
    {
        Destroy(gameObject); // Despawn the villager
    }

    public void StopFleeing()
    {
        aiPath.destination = transform.position; // Set destination to current position
        aiPath.maxSpeed = 0f;                    // Stop movement
        //aiPath.enabled = false;                  // Optionally, disable AIPath to stop recalculations
        enabled = false;                         // Disable this fleeing script
    }

    public void ResumeFleeing()
    {
        aiPath.enabled = true;
        aiPath.maxSpeed = fleeSpeed;
        enabled = true;

        // Resume pathfinding towards the entrance spot
        aiPath.destination = targetPosition;
        aiPath.SearchPath();
    }

    void OnDisable() {
        StopFleeing();
    }

    public void HandleHurt()
    {
        StartCoroutine(HurtCoroutine());
    }

    private IEnumerator HurtCoroutine()
    {
        StopFleeing();                        // Temporarily stop fleeing

        yield return new WaitForSeconds(1f);  // Wait for the animation to play (adjust duration as needed)

        ResumeFleeing();                      // Resume fleeing after the animation
    }
}
