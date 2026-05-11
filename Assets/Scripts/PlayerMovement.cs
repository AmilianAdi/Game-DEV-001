using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveDistance = 1f;
    public int attackDamage = 2;
    private PlayerActionPoints ap;
    private void Start()
    {
        ap = GetComponent<PlayerActionPoints>();
        Vector3Int gridPos = Vector3Int.FloorToInt(transform.position);
        GridManager.Instance.RegisterEntity(gameObject, gridPos);
    }

    private void Update()
    {
        if (!TurnManager.Instance.isPlayerTurn)
            return;
        if (ap != null && !ap.CanMove())
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
        if (ap != null && !ap.CanMove())
            return;
        Vector3 newPos = transform.position + direction * moveDistance;
        Vector3Int newGridPos = Vector3Int.FloorToInt(newPos);
        GameObject enemyOnTile = GetEnemyAtGridPosition(newGridPos);

        if (enemyOnTile != null)
        {
            Health hp = enemyOnTile.GetComponent<Health>();

            if (hp != null)
                hp.TakeDamage(attackDamage, DamageType.Melee);

            if (ap != null)
                ap.SpendMove();

            if (ap != null && ap.movesLeft <= 0)
                TurnManager.Instance.EndPlayerTurn();

            return;
        }

        if (GridManager.Instance.IsTileOccupied(newGridPos))
        {
            GameObject occupant = GridManager.Instance.GetEntityAt(newGridPos);

            if (occupant != null && occupant.CompareTag("Enemy"))
            {
                Health hp = occupant.GetComponent<Health>();
                if (hp != null) hp.TakeDamage(attackDamage, DamageType.Melee);
                if (ap != null) ap.SpendMove();
                if (ap != null && ap.movesLeft <= 0)
                    TurnManager.Instance.EndPlayerTurn();
            }
            return;
        }
        Vector3Int oldGridPos = Vector3Int.FloorToInt(transform.position);
        GridManager.Instance.UnregisterEntity(oldGridPos);
        transform.position = newPos;
        GridManager.Instance.RegisterEntity(gameObject, newGridPos);
        if (ap != null) ap.SpendMove();
        //if (ap != null && ap.movesLeft <= 0)
            //TurnManager.Instance.EndPlayerTurn();
    }
    private GameObject GetEnemyAtGridPosition(Vector3Int gridPos)
    {
        EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>();

        foreach (EnemyMovement enemy in enemies)
        {
            if (enemy == null)
                continue;

            Vector3Int enemyGridPos = Vector3Int.FloorToInt(enemy.transform.position);

            if (enemyGridPos.x == gridPos.x && enemyGridPos.z == gridPos.z)
            {
                return enemy.gameObject;
            }
        }

        return null;
    }
}
