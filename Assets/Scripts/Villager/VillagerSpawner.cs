using UnityEngine;

public class VillagerSpawner : MonoBehaviour
{
    [Header("Villager Settings")]
    public GameObject villagerPrefab;  // Assign the villager prefab in the Inspector
    public int villagerCount = 10;     // Number of villagers to spawn

    [Header("Spawn Area Settings")]
    public Vector2 spawnAreaCenter;    // Center of the area to spawn villagers
    public Vector2 spawnAreaSize;      // Width and height of the spawn area

    [Header("Spawn Settings")]
    public LayerMask obstacleLayer;    // LayerMask for buildings and obstacles
    public float minSpawnDistance = 1f; // Minimum distance from obstacles

    [Header("Parent Object")]
    public Transform villagersParent;  // Assign the "Villagers" GameObject as the parent in the Inspector

    void Start()
    {
        SpawnVillagers();
    }

    private void SpawnVillagers()
    {
        int spawnedVillagers = 0;

        // Attempt to spawn villagers until the specified count is reached
        while (spawnedVillagers < villagerCount)
        {
            Vector2 randomPosition = GetRandomPositionWithinArea();
            
            // Check if the position is clear (not overlapping an obstacle)
            if (IsPositionClear(randomPosition))
            {
                // Spawn the villager at the cleared position
                GameObject villager = Instantiate(villagerPrefab, randomPosition, Quaternion.identity, villagersParent);
                
                spawnedVillagers++;
            }
        }
    }

    private Vector2 GetRandomPositionWithinArea()
    {
        // Generate a random position within the defined spawn area
        float randomX = Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2);
        float randomY = Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2, spawnAreaCenter.y + spawnAreaSize.y / 2);
        return new Vector2(randomX, randomY);
    }

    private bool IsPositionClear(Vector2 position)
    {
        // Check if there's any obstacle within the minSpawnDistance radius
        Collider2D hitCollider = Physics2D.OverlapCircle(position, minSpawnDistance, obstacleLayer);
        return hitCollider == null; // Position is clear if there's no collider in the specified layer
    }
}
