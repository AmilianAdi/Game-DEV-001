using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Enemy Prefabs")]
    public GameObject basicSlimePrefab;
    public GameObject strongSlimePrefab;
    public GameObject bossSlimePrefab;

    [Header("Spawn Points")]
    public Transform[] enemySpawnPoints;

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
            //3 basic slimes
            SpawnEnemy(basicSlimePrefab, 0);
            SpawnEnemy(basicSlimePrefab, 1);
            SpawnEnemy(basicSlimePrefab, 2);
        }
        else if (roomNumber == 2)
        {
            //2slimebig and 1 small
            SpawnEnemy(strongSlimePrefab, 0);
            SpawnEnemy(strongSlimePrefab, 1);
            SpawnEnemy(basicSlimePrefab, 2);
        }
        else if (roomNumber == 3)
        {
            //boss
            SpawnEnemy(bossSlimePrefab, 1);
        }
    }

    private void SpawnEnemy(GameObject prefab, int spawnIndex)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Missing enemy prefab.");
            return;
        }

        if (enemySpawnPoints == null || spawnIndex >= enemySpawnPoints.Length || enemySpawnPoints[spawnIndex] == null)
        {
            Debug.LogWarning("Missing enemy spawn point.");
            return;
        }

        GameObject enemy = Instantiate(
            prefab,
            enemySpawnPoints[spawnIndex].position,
            Quaternion.identity
        );

        aliveEnemies.Add(enemy);
        SpawnHealthBar(enemy);
    }

    public void EnemyKilled(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);

        Debug.Log($"Enemy killed. Remaining enemies: {aliveEnemies.Count}");

        if (aliveEnemies.Count <= 0)
            RoomComplete();
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

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        Time.timeScale = 0f;
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