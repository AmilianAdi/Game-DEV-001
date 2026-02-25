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
        yield return new WaitForSeconds(0.5f);
        isPlayerTurn = true;
        Debug.Log("Player Turn");
    }
}
