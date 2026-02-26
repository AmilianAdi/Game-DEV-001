using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
        void Die()
        {
            Vector3Int gridPos = Vector3Int.FloorToInt(transform.position);
            GridManager.Instance.UnregisterEntity(gridPos);

            Destroy(gameObject);
        }
    
}
