using UnityEngine;

public class SlimeBob : MonoBehaviour
{
    public float speed = 2f;
    public float amount = 0.04f;
    private Vector3 startScale;
    private void Start()
    {
        startScale = transform.localScale;
    }
    private void Update()
    {
        float pulse = Mathf.Sin(Time.time * speed) * amount;
        transform.localScale = new Vector3(
            startScale.x + pulse,
            startScale.y - pulse,
            startScale.z + pulse
        );
    }
}
