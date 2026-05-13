using UnityEngine;

public class DamageNumberSpawner : MonoBehaviour
{
    public GameObject damageNumberPrefab;
    public Vector3 offset = new Vector3(0f, 1.2f, 0f);

    public void SpawnDamageNumber(int damageAmount)
    {
        if (damageNumberPrefab == null) return;

        GameObject numberObject = Instantiate(
            damageNumberPrefab,
            transform.position + offset,
            Quaternion.identity
        );
        DamageNumber damageNumber = numberObject.GetComponent<DamageNumber>();
        if (damageNumber != null)
        {
            damageNumber.Setup(damageAmount);
        }
    }
}
