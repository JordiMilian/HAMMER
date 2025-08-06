using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/LessHPMoreATK", fileName = "LessHPMoreATK")]
public class Upgrade_LessHPMoreAttack : Upgrade
{
    Player_References playerRefs;

    public override void onAdded(GameObject entity)
    {
        playerRefs = entity.GetComponent<Player_References>();
        playerRefs.currentStats.DamageMultiplicator += 1f;

        float removedValue = playerRefs.baseStats.MaxHp * .5f;
        playerRefs.currentStats.MaxHp -= removedValue;
        if(playerRefs.currentStats.CurrentHp > playerRefs.currentStats.MaxHp)
        {
            playerRefs.currentStats.CurrentHp = playerRefs.currentStats.MaxHp;
        }
    }

    public override void onRemoved(GameObject entity)
    {
        playerRefs.currentStats.DamageMultiplicator -= 1f;

        float addedValue = playerRefs.baseStats.MaxHp * .5f;
        playerRefs.currentStats.MaxHp += addedValue;
    }

    public override string shortDescription()
    {
        return "Half HP, Doble Attack";
    }
}
