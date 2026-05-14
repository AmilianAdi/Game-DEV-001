using UnityEngine;

public class RangeSpell : MonoBehaviour
{
    public GameObject spellProjectilePrefab;
    public int damage = 3;
    public float range = 6f;
    private bool isTargeting;
    public void StartTargeting()
    {
        isTargeting = true;
        Debug.Log("Select an enemy to hit.");
    }
    private void Update()
    {
        if (!isTargeting) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 200f))
            {
                var enemyHealth = hit.collider.GetComponentInParent<Health>();
                if (enemyHealth != null)
                {
                    float dist = Vector3.Distance(transform.position, enemyHealth.transform.position);
                    if (dist <= range)
                    {
                        enemyHealth.TakeDamage(damage, DamageType.Spell);
                        SpawnSpellProjectile(enemyHealth.transform.position);
                        Debug.Log("Spell hit!");
                        isTargeting = false;
                    }
                    else
                    {
                        Debug.Log("Out of range!");
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
            isTargeting = false;
    }
    private void SpawnSpellProjectile(Vector3 targetPosition)
    {
        if (spellProjectilePrefab == null) return;

        Vector3 startPosition = transform.position + Vector3.up * 0.8f;
        Vector3 endPosition = targetPosition + Vector3.up * 0.6f;

        GameObject projectile = Instantiate(
            spellProjectilePrefab,
            startPosition,
            Quaternion.identity
        );

        spellProjectile visual = projectile.GetComponent<spellProjectile>();

        if (visual != null)
        {
            visual.Setup(endPosition);
        }
    }
}
