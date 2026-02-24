using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveDistance = 1f;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position += Vector3.forward * moveDistance;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position += Vector3.back * moveDistance;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += Vector3.right * moveDistance;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += Vector3.left * moveDistance;
        }
    }
}
