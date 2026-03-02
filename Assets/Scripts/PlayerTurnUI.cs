using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerTurnUI : MonoBehaviour
{
    public PlayerActionPoints ap;
    public RangeSpell rangedSpell;
    public TextMeshProUGUI movesText;
    public TextMeshProUGUI spellText;
    public Button castSpellButton;
    public Button endTurnButton;
    private void OnEnable()
    {
        if (ap != null) ap.OnChanged += Refresh;
        Refresh();
        if (endTurnButton != null)
            endTurnButton.onClick.AddListener(() => TurnManager.Instance.EndPlayerTurn());

        if (castSpellButton != null)
            castSpellButton.onClick.AddListener(TryCastSpell);
    }
    private void OnDisable()
    {
        if (ap != null) ap.OnChanged -= Refresh;
    }
    void Refresh()
    {
        if (ap == null) return;
        movesText.text = $"Moves: {ap.movesLeft}/{ap.maxMovesPerTurn}";
        spellText.text = ap.CanCastSpell() ? "Spell: Ready" : "Spell: Used";
        bool playerTurn = TurnManager.Instance == null || TurnManager.Instance.isPlayerTurn;
        castSpellButton.interactable = playerTurn && ap.CanCastSpell();
        endTurnButton.interactable = playerTurn;
    }
    void TryCastSpell()
    {
        if (TurnManager.Instance != null && !TurnManager.Instance.isPlayerTurn) return;
        if (ap == null) return;

        if (!ap.SpendSpell())
            return;
        if (rangedSpell != null)
        {
            rangedSpell.StartTargeting();
        }
        else
        {
            Debug.LogError("PlayerTurnUI: rangedSpell is not assigned in Inspector!");
        }
        Refresh();
    }
}
