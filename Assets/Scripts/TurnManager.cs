using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public bool isPlayerTurn = true;
    private void Awake()
    {
        Debug.Log("TurnManager Loaded");
        Instance = this;
    }
    public void EndPlayerTurn()
    {
        isPlayerTurn = false;
        StartCoroutine(EnemyTurn());
    }
    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn");
        EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>();
        foreach (EnemyMovement enemy in enemies)
        {
            enemy.TakeTurn();
        }
        yield return new WaitForSeconds(0.3f);
        isPlayerTurn = true;
        Debug.Log("Player Turn");
    }
}
