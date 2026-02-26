using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveDistance = 1f;
    public int attackDamage = 2;
    private void Update()
    {
        if (!TurnManager.Instance.isPlayerTurn)
            return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Vector3.forward);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector3.back);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector3.right);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector3.left);
        }
    }
    void Move(Vector3 direction)
    {
        Vector3 newPos = transform.position + direction * moveDistance;
        Vector3Int newGridPos = Vector3Int.FloorToInt(newPos);
        if (GridManager.Instance.IsTileOccupied(newGridPos))
        {
            GameObject occupant = GridManager.Instance.GetEntityAt(newGridPos);
            if (occupant.CompareTag("Enemy"))
            {
                occupant.GetComponent<Health>().TakeDamage(attackDamage);
                TurnManager.Instance.EndPlayerTurn();
            }
            return;
        }
        Vector3Int oldGridPos = Vector3Int.FloorToInt(transform.position);
        GridManager.Instance.UnregisterEntity(oldGridPos);
        transform.position= newPos;
        GridManager.Instance.RegisterEntity(gameObject, newGridPos);
        TurnManager.Instance.EndPlayerTurn();
    }
}
