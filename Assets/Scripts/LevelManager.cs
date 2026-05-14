using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Enemy Prefabs")]
    public GameObject basicSlimePrefab;
    public GameObject strongSlimePrefab;
    public GameObject bossSlimePrefab;

    [Header("Random Spawning")]
    public Transform player;
    public float spawnY = 0.1f;

    public int minX = -5;
    public int maxX = 9;
    public int minZ = -6;
    public int maxZ = 8;

    public int minDistanceFromPlayer = 3;
    public int minDistanceBetweenEnemies = 1;
    public int maxSpawnAttempts = 100;

    [Header("Health Bars")]
    public GameObject healthBarPrefab;

    [Header("UI")]
    public GameObject victoryPanel;

    [Header("Progression")]
    public int currentRoom = 1;

    private List<GameObject> aliveEnemies = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
                player = playerObj.transform;
        }

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        StartRoom(1);
    }
    public void StartRoom(int roomNumber)
    {
        currentRoom = roomNumber;
        ClearOldEnemies();

        Debug.Log($"Starting Room {currentRoom}");

        if (roomNumber == 1)
        {
            //3noobs
            SpawnEnemy(basicSlimePrefab);
            SpawnEnemy(basicSlimePrefab);
            SpawnEnemy(basicSlimePrefab);
        }
        else if (roomNumber == 2)
        {
            //2strong slime 1 meh
            SpawnEnemy(strongSlimePrefab);
            SpawnEnemy(strongSlimePrefab);
            SpawnEnemy(basicSlimePrefab);
        }
        else if (roomNumber == 3)
        {
            //boss
            SpawnEnemy(bossSlimePrefab);
        }
        HealthBarSpawner spawner = FindObjectOfType<HealthBarSpawner>();

        if (spawner != null)
            spawner.RefreshAllHealthBars();
    }

    private void SpawnEnemy(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Missing enemy prefab.");
            return;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();

        GameObject enemy = Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity
        );

        aliveEnemies.Add(enemy);
        //SpawnHealthBar(enemy);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3Int playerGridPos = Vector3Int.zero;

        if (player != null)
            playerGridPos = Vector3Int.FloorToInt(player.position);

        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            int x = Random.Range(minX, maxX + 1);
            int z = Random.Range(minZ, maxZ + 1);
            Vector3Int candidateGridPos = new Vector3Int(x, 0, z);

            if (GridManager.Instance != null && GridManager.Instance.IsTileOccupied(candidateGridPos))
                continue;

            int distanceFromPlayer =
                Mathf.Abs(candidateGridPos.x - playerGridPos.x) +
                Mathf.Abs(candidateGridPos.z - playerGridPos.z);

            if (distanceFromPlayer < minDistanceFromPlayer)
                continue;

            if (!IsFarEnoughFromOtherEnemies(candidateGridPos))
                continue;
            return new Vector3(x + 0.5f, spawnY, z + 0.5f);
        }

        Debug.LogWarning("Could not find ideal random spawn. Using fallback position.");
        return new Vector3(5.5f, spawnY, 5.5f);
    }

    private bool IsFarEnoughFromOtherEnemies(Vector3Int candidateGridPos)
    {
        foreach (GameObject enemy in aliveEnemies)
        {
            if (enemy == null)
                continue;

            Vector3Int enemyGridPos = Vector3Int.FloorToInt(enemy.transform.position);

            int distance =
                Mathf.Abs(candidateGridPos.x - enemyGridPos.x) +
                Mathf.Abs(candidateGridPos.z - enemyGridPos.z);

            if (distance <= minDistanceBetweenEnemies)
                return false;
        }

        return true;
    }
    public void EnemyKilled(GameObject enemy)
    {
        Debug.Log($"LevelManager received death: {enemy.name}");

        aliveEnemies.Remove(enemy);
        aliveEnemies.RemoveAll(e => e == null);

        Debug.Log($"Enemies remaining: {aliveEnemies.Count}");

        if (aliveEnemies.Count <= 0)
        {
            RoomComplete();
        }
    }
    private void RoomComplete()
    {
        Debug.Log($"Room {currentRoom} complete.");

        if (currentRoom < 3)
        {
            if (UpgradeManager.Instance != null)
                UpgradeManager.Instance.ShowUpgradeChoices();
            else
                ContinueToNextRoom();
        }
        else
        {
            Victory();
        }
    }
    public void ContinueToNextRoom()
    {
        Time.timeScale = 1f;
        StartRoom(currentRoom + 1);
    }

    private void Victory()
    {
        Debug.Log("Boss defeated. Victory!");
        if (EndScrreen.Instance != null)
        {
            EndScrreen.Instance.ShowVictory();
        }
        else
        {
            Debug.LogWarning("EndScrreen.Instance is NULL. Falling back to old victory panel.");
            if (victoryPanel != null)
                victoryPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    private void ClearOldEnemies()
    {
        foreach (GameObject enemy in aliveEnemies)
        {
            if (enemy != null)
            {
                if (GridManager.Instance != null)
                    GridManager.Instance.UnregisterEntity(enemy);

                Destroy(enemy);
            }
        }
        aliveEnemies.Clear();
    }
    private void SpawnHealthBar(GameObject enemy)
    {
        if (healthBarPrefab == null)
            return;

        Health enemyHealth = enemy.GetComponent<Health>();

        if (enemyHealth == null)
            return;

        GameObject bar = Instantiate(healthBarPrefab);

        HealthBarUI barUI = bar.GetComponent<HealthBarUI>();

        if (barUI != null)
            barUI.Bind(enemyHealth);
    }
}