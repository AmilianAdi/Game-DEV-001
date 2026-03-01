using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    public System.Action<int, int> OnHealthChanged;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        damage = Mathf.Max(0, damage);
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        Debug.Log($"{gameObject.name} took {damage} damage. HP: {currentHealth}/{maxHealth}");
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
        if (CompareTag("Player"))
        {
            Debug.Log("GAME OVER");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        Destroy(gameObject);
    }
    
}
