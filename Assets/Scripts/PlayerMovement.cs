using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveDistance = 1f;
    public int attackDamage = 2;
    [Header("Camera Relative Movement")]
    public Transform cameraPivot;
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
            Move(GetCameraRelativeDirection(Vector3.forward));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(GetCameraRelativeDirection(Vector3.back));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Move(GetCameraRelativeDirection(Vector3.right));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(GetCameraRelativeDirection(Vector3.left));
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
    private Vector3 GetCameraRelativeDirection(Vector3 inputDirection)
    {
        if (cameraPivot == null)
        {
            return inputDirection;
        }
        Vector3 forward = cameraPivot.forward;
        Vector3 right = cameraPivot.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        Vector3 direction = (forward * inputDirection.z) + (right * inputDirection.x);
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            direction = new Vector3(Mathf.Sign(direction.x), 0f, 0f);
        }
        else
        {
            direction = new Vector3(0f, 0f, Mathf.Sign(direction.z));
        }
        return direction;
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
