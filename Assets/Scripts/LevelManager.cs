using System.Collections.Generic;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Enemy Prefabs")]
    public GameObject slimePrefab;
    public GameObject lavaBatPrefab;
    public GameObject bossSlimePrefab;

    [Header("Spawn Points")]
    public Transform[] enemySpawnPoints;

    [Header("Health Bars")]
    public GameObject healthBarPrefab;

    [Header("Progression")]
    public int currentRoom = 1;
    public int newGamePlusLevel = 0;

    private List<GameObject> aliveEnemies = new List<GameObject>();
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        StartRoom(1);
    }
    public void StartRoom(int roomNumber)
    {
        currentRoom = roomNumber;
        ClearOldEnemies();
        Debug.Log($"Starting Room {currentRoom}. New Game+ Level: {newGamePlusLevel}");
        if (roomNumber == 1)
        {
            SpawnEnemy(slimePrefab, 0);
            SpawnEnemy(slimePrefab, 1);
            SpawnEnemy(slimePrefab, 2);
        }
        else if (roomNumber == 2)
        {
            SpawnEnemy(lavaBatPrefab, 0);
            SpawnEnemy(lavaBatPrefab, 1);
            SpawnEnemy(slimePrefab, 2);
        }
        else if (roomNumber == 3)
        {
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

        if (spawnIndex >=enemySpawnPoints.Length || enemySpawnPoints[spawnIndex] == null)
        {
            Debug.LogWarning("Missing enemy spawn point.");
            return;
        }

        GameObject enemy = Instantiate(prefab, enemySpawnPoints[spawnIndex].position, Quaternion.identity);
        aliveEnemies.Add(enemy);

        ScaleEnemy(enemy);
        SpawnHealthBar(enemy);
    }

    private void ScaleEnemy(GameObject enemy)
    {
        Health health = enemy.GetComponent<Health>();

        if (health != null)
        {
            float hpMultiplier =Mathf.Pow(1.4f, newGamePlusLevel);
            health.maxHealth = Mathf.RoundToInt(health.maxHealth * hpMultiplier);
            health.currentHealth= health.maxHealth;
            health.OnHealthChanged?.Invoke(health.currentHealth, health.maxHealth);
        }
        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();

        if (movement != null)
        {
            float damageMultiplier = Mathf.Pow(1.3f, newGamePlusLevel);
            movement.attackDamage = Mathf.RoundToInt(movement.attackDamage * damageMultiplier);
        }
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
        Debug.Log("Room complete. Choose an upgrade.");

        if (UpgradeManager.Instance != null)
            UpgradeManager.Instance.ShowUpgradeChoices();
        else
            ContinueToNextRoom();
    }

    public void ContinueToNextRoom()
    {
        if (currentRoom < 3)
            StartRoom(currentRoom + 1);
        else
            StartNewGamePlus();
    }
    private void StartNewGamePlus()
    {
        newGamePlusLevel++;
        Debug.Log($"Boss defeated. Starting New Game+ {newGamePlusLevel}");
        StartRoom(1);
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