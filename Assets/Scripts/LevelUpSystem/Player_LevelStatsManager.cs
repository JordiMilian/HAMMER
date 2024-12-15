using UnityEngine; 

public class Player_LevelStatsManager : MonoBehaviour
{
    //SerializeField] PlayerStats currentStats;
    //[SerializeField] PlayerStats baseStats;
    [SerializeField] Player_References playerRefs;
    [Space]
    [Header("Level up Settings")]
    [SerializeField] int baseLevelCost;
    [Tooltip("The total cost per level is calculated as (baseLevelCost * level) * costMult")]
    [SerializeField] float costMult = 1.5f;

    [Space]
    [Header("Stats upgrades per level")]
    [SerializeField] int hpPerLevel = 1;
    [SerializeField] float damagePerLevel = 1;
    [SerializeField] int staminaPerLevel = 1;

    public void LevelUpHP()
    {
        if (!canLevelUp()) return; //No puede subir de nivel

        Debug.Log("Level up! " + playerRefs.currentStats.Level);
        Debug.Log("Level Up Hp");
        int currentMaxHealth = Mathf.RoundToInt(playerRefs.currentStats.MaxHp);
        int currentHealth = Mathf.RoundToInt(playerRefs.currentStats.CurrentHp);

        int newMaxHealth = (currentMaxHealth + hpPerLevel);
        playerRefs.currentStats.MaxHp = newMaxHealth;
        playerRefs.currentStats.CurrentHp = currentHealth + hpPerLevel;

        playerRefs.currentStats.CurrentHp = newMaxHealth;

        playerRefs.currentStats.MaxHp = newMaxHealth;
    }

    public void LevelUpDamage()
    {
        if (!canLevelUp()) return; //No puede subir de nivel

        Debug.Log("Level up! " + playerRefs.currentStats.Level);
        Debug.Log("Level Up Damage");
        float currentDamage = playerRefs.currentStats.DamageMultiplicator;

        float newDamage = (currentDamage + damagePerLevel);

        playerRefs.currentStats.DamageMultiplicator = newDamage;


    }

    public void LevelUpStamina()
    {
        if (!canLevelUp()) return; //No puede subir de nivel

        Debug.Log("Level up! " + playerRefs.currentStats.Level);
        Debug.Log("Level Up Stamina");
        int currentStaminaMax = Mathf.RoundToInt(playerRefs.currentStats.MaxStamina);
        int currentStamina = Mathf.RoundToInt(playerRefs.currentStats.CurrentStamina);

        int newMaxStamina = (currentStaminaMax + staminaPerLevel);
        playerRefs.currentStats.MaxStamina = currentStaminaMax + staminaPerLevel;
        playerRefs.currentStats.CurrentStamina = currentStamina + staminaPerLevel;

        playerRefs.currentStats.MaxStamina = newMaxStamina;
    }
    bool canLevelUp()
    {
        int xpPoints = playerRefs.currentStats.ExperiencePoints;

        return xpPoints - levelUpCost() >= 0 ? true: false;
    }

    public int xpPoints()
    {
        return playerRefs.currentStats.ExperiencePoints;
    }
    public int levelUpCost()
    {
        if (playerRefs.currentStats.Level <= 0) playerRefs.currentStats.Level = 1;

        return levelUpCostCalculation(playerRefs.currentStats.Level);
    }

    public int levelUpCostCalculation(int level)
    {
        return Mathf.RoundToInt(baseLevelCost * Mathf.Pow(costMult, level - 1));
    }
    public void ResetLevel1()
    {
        playerRefs.currentStats.CopyData(playerRefs.baseStats);
    }

    public int GetHPLevel()
    {
        return Mathf.FloorToInt((playerRefs.currentStats.MaxHp - playerRefs.baseStats.MaxHp) / hpPerLevel);
    }

    public int GetStaminaLevel()
    {
        return Mathf.FloorToInt((playerRefs.currentStats.MaxStamina - playerRefs.baseStats.MaxStamina) / staminaPerLevel);
    }

    public int GetDamageLevel()
    {
        return Mathf.FloorToInt((playerRefs.currentStats.DamageMultiplicator - playerRefs.baseStats.DamageMultiplicator) / damagePerLevel);
    }
}
