using UnityEngine;

public class spellProjectile : MonoBehaviour
{
    public float speed = 50f;
    public float lifetime = 1.5f;
    private Vector3 targetPosition;
    private float timer;
    public void Setup(Vector3 target)
    {
        targetPosition = target;
    }
    private void Update()
    {
        timer += Time.deltaTime;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
        Vector3 direction = targetPosition - transform.position;
        if (direction.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
        if (Vector3.Distance(transform.position, targetPosition) < 0.05f || timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
