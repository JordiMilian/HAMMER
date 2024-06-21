using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Max HP")]
public class Upgrade_MaxHP : Upgrade
{
    [SerializeField] float Percent;
    Player_HealthSystem playerHealth;
    public override void onAdded(GameObject entity)
    {
        playerHealth = entity.GetComponent<Player_HealthSystem>();
        float newValue = playerHealth.MaxHP.GetValue() * (1 + (Percent/100));
        playerHealth.MaxHP.SetValue(newValue);

    }
    public override void onRemoved(GameObject entity)
    {
        float newValue = playerHealth.MaxHP.GetValue() / (1 + (Percent/100));
        playerHealth.MaxHP.SetValue(newValue);
    }
}
[CreateAssetMenu(menuName = "Upgrades/Weapon Size")]
public class Upgrade_WeaponSize : Upgrade
{
    [SerializeField] float Percent;
    GameObject playerWeapon;
    public override void onAdded(GameObject entity)
    {
        playerWeapon = entity.GetComponent<Player_References>().weaponScalingRoot;
        float percentMultiplier = 1 + (Percent/100);
        playerWeapon.transform.localScale = new Vector3(
            playerWeapon.transform.localScale.x * percentMultiplier,
            playerWeapon.transform.localScale.y * percentMultiplier,
            playerWeapon.transform.localScale.z
            );
    }
    public override void onRemoved(GameObject entity)
    {
        float percentMultiplier = 1 + (Percent / 100);
        playerWeapon.transform.localScale = new Vector3(
            playerWeapon.transform.localScale.x / percentMultiplier,
            playerWeapon.transform.localScale.y / percentMultiplier,
            playerWeapon.transform.localScale.z
            );
    }
}
[CreateAssetMenu (menuName = "Upgrades/Slower but Stronger attacks")]
public class Upgrade_SlowerStrongerAttacks : Upgrade
{
    [SerializeField] float SlowerPercent;
    [SerializeField] float StrongerPercent;
    Generic_Stats playerStats;
    public override void onAdded(GameObject entity)
    {
        playerStats = entity.GetComponent<Player_References>().stats;
        playerStats.DamageMultiplier *= 1 + (StrongerPercent / 100);
        //FALTE CONTROLADOR DE VELOCITAT DE ATACS
    }
    public override void onRemoved(GameObject entity)
    {
        playerStats.DamageMultiplier /= 1 + (StrongerPercent / 100);
    }
}
[CreateAssetMenu(menuName = "Upgrades/Max Stamina")]
public class Upgrade_MaxStamina : Upgrade
{
    FloatVariable playerStamina;
    [SerializeField] float Percent;
    public override void onAdded(GameObject entity)
    {
        playerStamina = entity.GetComponent<Player_References>().maxStamina;
        playerStamina.SetValue (playerStamina.GetValue() * (1 + (Percent / 100)));
    }
    public override void onRemoved(GameObject entity)
    {
        playerStamina.SetValue(playerStamina.GetValue() / (1 + (Percent / 100)));
    }
}
[CreateAssetMenu (menuName = "Upgrades/Faster but Weaker Attacks")]
public class Upgrade_FasterWeakerAttacks : Upgrade
{
    [SerializeField] float FasterPercent;
    [SerializeField] float WeakerPercent;
    Generic_Stats playerStats;
    public override void onAdded(GameObject entity)
    {
        playerStats = entity.GetComponent<Player_References>().stats;
        playerStats.DamageMultiplier /= 1 + (WeakerPercent / 100);
        //ATTACKS ANIMATIONS!!!
    }
    public override void onRemoved(GameObject entity)
    {
        playerStats.DamageMultiplier *= 1 + (WeakerPercent / 100);
    }
}
