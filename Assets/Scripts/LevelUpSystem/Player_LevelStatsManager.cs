using UnityEngine; 

public class Player_LevelStatsManager : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [Space]
    [Header("Level up Settings")]
    [SerializeField] int baseLevelCost;
    [Tooltip("The total cost per level is calculated as (baseLevelCost * level) * costMult")]
    [SerializeField] float costMult = 1.5f;

    [Space]
    [Header("Stats upgrades per level")]
    [SerializeField] int hpPerLevel = 1;
    [SerializeField] int damagePerLevel = 1;
    [SerializeField] int staminaPerLevel = 1;

    [SerializeField] EntityStats currentPlayerStats;
    public void LevelUpHP()
    {
        if (!canLevelUp()) return; //No puede subir de nivel

        gameState.XpPoints -= levelUpCost();
        gameState.level++;

        Debug.Log("Level up! " + gameState.level);
        Debug.Log("Level Up Hp");
        int currentMaxHealth = Mathf.RoundToInt(GlobalPlayerReferences.Instance.references.maxHealth.Value);
        int currentHealth = Mathf.RoundToInt(GlobalPlayerReferences.Instance.references.currentHealth.Value);

        int newMaxHealth = (currentMaxHealth + hpPerLevel);
        GlobalPlayerReferences.Instance.references.maxHealth.SetValue(newMaxHealth);
        GlobalPlayerReferences.Instance.references.currentHealth.SetValue(currentHealth + hpPerLevel);

        gameState.level_currentMaxHp = newMaxHealth;

        currentPlayerStats.MaxHp = newMaxHealth;
    }

    public void LevelUpDamage()
    {
        if (!canLevelUp()) return; //No puede subir de nivel

        gameState.XpPoints -= levelUpCost();
        gameState.level++;

        Debug.Log("Level up! " + gameState.level);
        Debug.Log("Level Up Stamina");
        int currentDamage = Mathf.RoundToInt(GetComponent<Generic_Stats>().DamageMultiplier);

        int newDamage = (currentDamage + damagePerLevel);

        GetComponent<Generic_Stats>().DamageMultiplier = newDamage;


    }

    public void LevelUpStamina()
    {
        if (!canLevelUp()) return; //No puede subir de nivel

        gameState.XpPoints -= levelUpCost();
        gameState.level++;

        Debug.Log("Level up! " + gameState.level);
        Debug.Log("Level Up Stamina");
        int currentStaminaMax = Mathf.RoundToInt(GlobalPlayerReferences.Instance.references.maxStamina.Value);
        int currentStamina = Mathf.RoundToInt(GlobalPlayerReferences.Instance.references.currentStamina.Value);

        int newMaxStamina = (currentStaminaMax + staminaPerLevel);
        GlobalPlayerReferences.Instance.references.maxStamina.SetValue(currentStaminaMax + staminaPerLevel);
        GlobalPlayerReferences.Instance.references.currentStamina.SetValue(currentStamina + staminaPerLevel);

        gameState.level_currentMaxStamina = newMaxStamina;
    }
    bool canLevelUp()
    {
        int xpPoints = gameState.XpPoints;

        return xpPoints - levelUpCost() >= 0 ? true: false;
    }

    public int xpPoints()
    {
        return gameState.XpPoints;
    }
    public int levelUpCost()
    {
        if (gameState.level <= 0) gameState.level = 1;

        return Mathf.RoundToInt(baseLevelCost * Mathf.Pow(costMult, gameState.level -1));
    }

    public void ResetLevel1()
    {
        gameState.level = 1;
        gameState.XpPoints = 0;
        gameState.level_currentMaxHp = GlobalPlayerReferences.Instance.references.baseHealth.Value;
        //gameState.level_currentDamge = hay que mover esto a float reference 
        gameState.level_currentMaxStamina = GlobalPlayerReferences.Instance.references.baseStamina.Value;

        GlobalPlayerReferences.Instance.references.maxHealth.SetValue(gameState.level_currentMaxHp);
        GlobalPlayerReferences.Instance.references.currentHealth.SetValue(gameState.level_currentMaxHp);

        GlobalPlayerReferences.Instance.references.maxStamina.SetValue(gameState.level_currentMaxStamina);
        GlobalPlayerReferences.Instance.references.currentStamina.SetValue(gameState.level_currentMaxStamina);

        GetComponent<Generic_Stats>().DamageMultiplier = 1; //No utilizo la referencia a BaseDamage en Generic_Stats por si la quitas, que esto se mantenga como 1, ya que ese numero siempre es 1
    }


}
