using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float lifetime = 0.8f;

    private TextMeshPro text;
    private float timer;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount)
    {
        if (text == null)
        {
            text = GetComponent<TextMeshPro>();
        }

        if (text != null)
        {
            text.text = "-" + damageAmount.ToString();
        }
    }
    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }

        timer += Time.deltaTime;

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
