using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveDistance = 1f;
    public Transform player;

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
        transform.position += direction * moveDistance;
     }
}
