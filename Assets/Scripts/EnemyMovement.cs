using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveDistance = 1f;
    public Transform player;
    public int attackDamage = 1;

    private void Start()
    {
        Vector3Int gridPos = Vector3Int.FloorToInt(transform.position);
        GridManager.Instance.RegisterEntity(gameObject, gridPos);
    }

    public void TakeTurn() { 
    Vector3 direction = player.position - transform.position;
    if(Mathf.Abs(direction.x)>Mathf.Abs(direction.z))
        {
            direction = new Vector3(Mathf.Sign(direction.x), 0, 0);
        }
        else
        {
            direction = new Vector3(0, 0, Mathf.Sign(direction.z));
        }
        Vector3 newPos = transform.position + direction * moveDistance;
        Vector3Int newGridPos = Vector3Int.FloorToInt(newPos);

        if (GridManager.Instance.IsTileOccupied(newGridPos))
        {
            GameObject occupant = GridManager.Instance.GetEntityAt(newGridPos);
            if (occupant.CompareTag("Player"))
            {
                occupant.GetComponent<Health>().TakeDamage(attackDamage);
            }
            return;
        }
        Vector3Int oldGridPos = Vector3Int.FloorToInt(transform.position);
        GridManager.Instance.UnregisterEntity(oldGridPos);
        transform.position = newPos;
        GridManager.Instance.RegisterEntity(gameObject, newGridPos);
    }
}
