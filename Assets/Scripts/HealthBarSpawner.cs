using UnityEngine;

public class HealthBarSpawner : MonoBehaviour
{
    public GameObject healthBarPrefab;
    public void RefreshAllHealthBars()
    {
        if (healthBarPrefab == null)
        {
            Debug.LogWarning("Health bar prefab missing.");
            return;
        }
        HealthBarUI[] oldBars = FindObjectsOfType<HealthBarUI>();

        foreach (HealthBarUI bar in oldBars)
        {
            if (bar != null)
            {
                Destroy(bar.gameObject);
            }
        }
        Health[] allHealth = FindObjectsOfType<Health>();
        foreach (Health h in allHealth)
        {
            if (h != null)
            {
                SpawnFor(h);
            }
        }
    }
    private void SpawnFor(Health h)
    {
        GameObject hb = Instantiate(healthBarPrefab);

        HealthBarUI ui = hb.GetComponent<HealthBarUI>();

        if (ui == null)
            ui = hb.AddComponent<HealthBarUI>();

        ui.slider = hb.GetComponentInChildren<UnityEngine.UI.Slider>(true);
        ui.Bind(h);
    }
}