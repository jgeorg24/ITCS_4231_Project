using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Public variables
    public GameObject enemyPrefab;
    public Transform ground;
    public Transform player;
    public float spawnRadius = 10f;
    public float minDistanceFromPlayer = 5f;
    public float yOffset = 1f;
    public char spawnKey = 'Y';

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(GetKeyCodeFromChar(spawnKey)))
        {
            SpawnEnemy();
        }
    }

    // Function to convert char to KeyCode
    KeyCode GetKeyCodeFromChar(char character)
    {
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), character.ToString());
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 randomDirection = new Vector3(randomCircle.x, 0f, randomCircle.y);

        Vector3 spawnPosition = player.position + randomDirection;

        // Ensure the enemy spawns above the ground
        spawnPosition.y = player.position.y + yOffset;

        // Check if spawn position is too close to the player
        if (Vector3.Distance(spawnPosition, player.position) < minDistanceFromPlayer)
        {
            Debug.Log("Spawn position too close to player. Finding new position.");

            // Find a new spawn position
            randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
            randomDirection = new Vector3(randomCircle.x, 0f, randomCircle.y);
            spawnPosition = player.position + randomDirection;
            spawnPosition.y = player.position.y + yOffset;

            // Check if new spawn position is in front of player's view
            Vector3 playerToSpawn = spawnPosition - player.position;
            if (Vector3.Dot(playerToSpawn, player.forward) > 0f)
            {
                // If spawn position is in front of player, adjust it
                spawnPosition = player.position + player.forward * minDistanceFromPlayer * 2f;
            }
        }

        Debug.Log("Spawn position: " + spawnPosition);

        return spawnPosition;
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}