using UnityEngine;

public class PlayerActionPoints : MonoBehaviour
{
    [Header("Per turn limits")]
    public int maxMovesPerTurn = 3;
    public int maxSpellsPerTurn = 1;
    [Header("Runtime")]
    public int movesLeft;
    public int spellsLeft;
    public System.Action OnChanged;
    private void Awake()
    {
        ResetForNewTurn();
    }

    public void ResetForNewTurn()
    {
        movesLeft = maxMovesPerTurn;
        spellsLeft = maxSpellsPerTurn;
        OnChanged?.Invoke();
    }
    public bool CanMove() => movesLeft > 0;
    public bool CanCastSpell() => spellsLeft > 0;

    public bool SpendMove()
    {
        if (!CanMove()) return false;
        movesLeft--;
        OnChanged?.Invoke();
        return true;
    }
    public bool SpendSpell()
    {
        if (!CanCastSpell()) return false;
        spellsLeft--;
        OnChanged?.Invoke();
        return true;
    }
}
