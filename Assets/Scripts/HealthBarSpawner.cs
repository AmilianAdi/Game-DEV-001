using UnityEngine;

public class HealthBarSpawner : MonoBehaviour
{
    public GameObject healthBarPrefab;
    private void Start()
    {
        SpawnForAll();
    }
    public void SpawnForAll()
    {
        Health[] all = FindObjectsOfType<Health>();
        foreach (Health h in all)
        {
            SpawnFor(h);
        }
    }
    void SpawnFor(Health h)
    {
        GameObject hb = Instantiate(healthBarPrefab);
        HealthBarUI ui = hb.GetComponent<HealthBarUI>();
        if (ui == null) ui = hb.AddComponent<HealthBarUI>();
        ui.target = h;
        if (ui.slider == null)
        {
            ui.slider = hb.GetComponentInChildren<UnityEngine.UI.Slider>();
        }
    }
}
