using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Health target;
    public Slider slider;
    public Vector3 offset = new Vector3(0, 2f, 0);
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;
        if (slider == null)
            slider = GetComponentInChildren<Slider>();
        if (target != null && slider != null)
        {
            slider.maxValue = target.maxHealth;
            slider.value = target.currentHealth;
            target.OnHealthChanged += HandleHealthChanged;
        }
    }
    private void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.transform.position + offset;

        if (cam != null)
            transform.forward = cam.transform.forward;
    }
    private void HandleHealthChanged(int current,int max)
    {
        slider.maxValue = max;
        slider.value = current;
    }

    private void OnDestroy()
    {
        if (target != null)
            target.OnHealthChanged -= HandleHealthChanged;
    }
}
