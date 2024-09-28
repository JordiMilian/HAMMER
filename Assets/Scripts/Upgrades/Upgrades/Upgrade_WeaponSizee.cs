using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Weapon Size")]
public class Upgrade_WeaponSizee : Upgrade
{
    [SerializeField] float Percent;
    GameObject playerWeapon;
    Generic_Stats stats;
    public override void onAdded(GameObject entity)
    {
        playerWeapon = entity.GetComponent<Player_References>().weaponScalingRoot;
        stats = entity.GetComponent<Generic_Stats>();

        float addedScale = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
        playerWeapon.transform.localScale = new Vector3(
            playerWeapon.transform.localScale.x + addedScale,
            playerWeapon.transform.localScale.y + addedScale,
            playerWeapon.transform.localScale.z
            );

        float StrongerPercent = Percent / 3;
        float addedDamage = stats.BaseDamageMultiplier * UsefullMethods.normalizePercentage(StrongerPercent, false, true);
        stats.DamageMultiplier += addedDamage;
    }
    public override void onRemoved(GameObject entity)
    {

        float addedScale = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
        playerWeapon.transform.localScale = new Vector3(
            playerWeapon.transform.localScale.x - addedScale,
            playerWeapon.transform.localScale.y - addedScale,
            playerWeapon.transform.localScale.z
            );

        float StrongerPercent = Percent / 3;
        float removedDamage = stats.BaseDamageMultiplier * UsefullMethods.normalizePercentage(StrongerPercent, false, true);
        stats.DamageMultiplier -= removedDamage;
    }
    public override string shortDescription()
    {
        return UsefullMethods.highlightString(Percent.ToString() + "%")
            + " bigger Weapon ";
    }
}
