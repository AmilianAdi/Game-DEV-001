using UnityEngine;

public class RangeSpell : MonoBehaviour
{
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
                        enemyHealth.TakeDamage(damage);
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
        if (Input.GetMouseButtonDown(1)) // right click cancel
            isTargeting = false;
    }
}
