using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    [Header("UI")]
    public GameObject upgradePanel;

    [Header("Player References")]
    public PlayerMovement playerMovement;
    public RangeSpell rangeSpell;
    public PlayerActionPoints playerActionPoints;
    public Health playerHealth;

    private void Awake()
    {
        Instance = this;

        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }
    public void ShowUpgradeChoices()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(true);

        Time.timeScale = 0f;
    }
    public void UpgradeMeleeDamage()
    {
        if (playerMovement != null)
            playerMovement.attackDamage += 1;

        Debug.Log("Upgrade chosen: +1 Melee Damage");

        CloseUpgradePanel();
    }
    public void UpgradeSpellDamage()
    {
        if (rangeSpell != null)
            rangeSpell.damage += 1;

        Debug.Log("Upgrade chosen: +1 Spell Damage");

        CloseUpgradePanel();
    }
    public void UpgradeMovement()
    {
        if (playerActionPoints != null)
        {
            playerActionPoints.maxMovesPerTurn += 1;
            playerActionPoints.ResetForNewTurn();
        }

        Debug.Log("Upgrade chosen: +1 Movement Per Turn");

        CloseUpgradePanel();
    }
    public void UpgradeMaxHealth()
    {
        if (playerHealth != null)
        {
            playerHealth.maxHealth += 3;
            playerHealth.currentHealth = playerHealth.maxHealth;
            playerHealth.OnHealthChanged?.Invoke(playerHealth.currentHealth, playerHealth.maxHealth);
        }

        Debug.Log("Upgrade chosen: +3 Max Health");

        CloseUpgradePanel();
    }

    public void UpgradeSpellUses()
    {
        if (playerActionPoints != null)
        {
            playerActionPoints.maxSpellsPerTurn += 1;
            playerActionPoints.ResetForNewTurn();
        }
        Debug.Log("Upgrade chosen: +1 Spell Use Per Turn");
        CloseUpgradePanel();
    }
    private void CloseUpgradePanel()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        Time.timeScale = 1f;

        if (LevelManager.Instance != null)
            LevelManager.Instance.ContinueToNextRoom();
    }
}