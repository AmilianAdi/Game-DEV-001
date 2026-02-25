using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveDistance = 1f;
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
        transform.position += direction * moveDistance;
        TurnManager.Instance.EndPlayerTurn();
    }
}
