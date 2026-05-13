using UnityEngine;
using System.Collections;

public class dmgFlash : MonoBehaviour
{
    public Renderer targetRenderer;
    public Color flashColor = Color.white;
    public float flashDuration = 0.2f;

    private Material[] materialInstances;
    private Color[] originalColors;
    private Coroutine flashCoroutine;
    private void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponentInChildren<Renderer>();
        }

        if (targetRenderer != null)
        {
            materialInstances = targetRenderer.materials;
            originalColors = new Color[materialInstances.Length];

            for (int i = 0; i < materialInstances.Length; i++)
            {
                originalColors[i] = materialInstances[i].color;
            }
        }
        else
        {
            Debug.LogWarning("No renderer found for damage flash on " + gameObject.name);
        }
    }
    public void Flash()
    {
        Debug.Log("Flash called on " + gameObject.name);

        if (materialInstances == null || materialInstances.Length == 0) return;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashRoutine());
    }
    private IEnumerator FlashRoutine()
    {
        for (int i = 0; i < materialInstances.Length; i++)
        {
            materialInstances[i].color = flashColor;
        }
        yield return new WaitForSeconds(flashDuration);
        for (int i = 0; i < materialInstances.Length; i++)
        {
            materialInstances[i].color = originalColors[i];
        }
        flashCoroutine = null;
    }
}
