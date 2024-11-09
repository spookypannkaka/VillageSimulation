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

    /*void OnEnable()
    {
        if (availableSpots.Count == 0)
        {
            Debug.LogWarning("No entrance spots available!");
            return;
        }

        int randomIndex = Random.Range(0, availableSpots.Count);
        Transform chosenSpot = availableSpots[randomIndex];
        targetPosition = chosenSpot.position;
        availableSpots.RemoveAt(randomIndex);

        // Set the target position for A* pathfinding
        aiPath.destination = targetPosition;
        aiPath.SearchPath(); // Force recalculation of the path
    }*/

    void OnEnable()
    {
        if (availableSpots.Count == 0)
        {
            Debug.LogWarning("No entrance spots available!");
            return;
        }

        aiPath.maxSpeed = fleeSpeed;

        // Choose a random EntranceSpot and get its collider's center
        int randomIndex = Random.Range(0, availableSpots.Count);
        Transform chosenSpot = availableSpots[randomIndex];
        availableSpots.RemoveAt(randomIndex);

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
}
