using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    [Header("Identity")]
    public bool isEnemy;
    public bool isPlayer;
    public EnemyType enemyType = EnemyType.Slime;

    public System.Action<int, int> OnHealthChanged;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        TakeDamage(damage, DamageType.Melee);
    }

    public void TakeDamage(int damage, DamageType damageType)
    {
        damage = Mathf.Max(0, damage);

        if (isEnemy && enemyType == EnemyType.LavaBat && damageType == DamageType.Melee)
        {
            Debug.Log($"{gameObject.name} resisted melee damage. Lava Bats can only be damaged by spells.");
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        Debug.Log($"{gameObject.name} took {damage} {damageType} damage. HP: {currentHealth}/{maxHealth}");

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (GridManager.Instance != null)
        {
            Vector3Int gridPos = Vector3Int.FloorToInt(transform.position);
            GridManager.Instance.UnregisterEntity(gridPos);
        }

        if (isPlayer || CompareTag("Player"))
        {
            Debug.Log("GAME OVER");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        if (isEnemy || CompareTag("Enemy"))
        {
            if (LevelManager.Instance != null)
                LevelManager.Instance.EnemyKilled(gameObject);

            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);
    }

}
