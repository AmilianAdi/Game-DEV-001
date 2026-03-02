using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Health target;
    public Slider slider;
    public Vector3 offset = new Vector3(0, 2f, 0);
    private Camera cam;
    private void OnEnable()
    {
        cam = Camera.main;

        if (slider == null)
            slider = GetComponentInChildren<Slider>(true);
    }
    public void Bind(Health h)
    {
        if (target != null)
            target.OnHealthChanged -= HandleHealthChanged;

        target = h;
        if (target != null && slider != null)
        {
            slider.maxValue = target.maxHealth;
            slider.value = target.currentHealth;
            target.OnHealthChanged += HandleHealthChanged;
        }
    }
    private void LateUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = target.transform.position + offset;
        if (cam == null) cam = Camera.main;
        if (cam != null)
            transform.forward = cam.transform.forward;
    }
    private void HandleHealthChanged(int current,int max)
    {
        if (slider == null) return;
        slider.maxValue = max;
        slider.value = current;
        if (current <= 0)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (target != null)
            target.OnHealthChanged -= HandleHealthChanged;
    }
}
