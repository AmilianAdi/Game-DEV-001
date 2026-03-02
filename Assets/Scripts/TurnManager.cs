using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public bool isPlayerTurn = true;
    public PlayerActionPoints playerAP;
    private void Awake()
    {
        Debug.Log("TurnManager Loaded");
        Instance = this;
    }
    private void Start()
    {
        if (playerAP != null)
            playerAP.ResetForNewTurn();
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
        if (playerAP != null)
            playerAP.ResetForNewTurn();
        Debug.Log("Player Turn");
    }
}
