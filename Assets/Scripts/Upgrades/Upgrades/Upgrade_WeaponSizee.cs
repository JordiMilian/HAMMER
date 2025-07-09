using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Weapon Size")]
public class Upgrade_WeaponSizee : Upgrade
{
    [SerializeField] float Percent;
    GameObject playerWeapon;
    Player_References playerRefs;
    public override void onAdded(GameObject entity)
    {
        playerWeapon = entity.GetComponent<Player_References>().weaponScalingRoot;
        playerRefs = entity.GetComponent<Player_References>();

        float addedScale = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
        playerWeapon.transform.localScale = new Vector3(
            playerWeapon.transform.localScale.x + addedScale,
            playerWeapon.transform.localScale.y + addedScale,
            playerWeapon.transform.localScale.z
            );

        float StrongerPercent = Percent / 3;
        float addedDamage = playerRefs.baseStats.DamageMultiplicator * UsefullMethods.normalizePercentage(StrongerPercent, false, true);
        playerRefs.currentStats.DamageMultiplicator += addedDamage;
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
        float removedDamage = playerRefs.baseStats.DamageMultiplicator * UsefullMethods.normalizePercentage(StrongerPercent, false, true);
        playerRefs.currentStats.DamageMultiplicator -= removedDamage;
    }
    public override string shortDescription()
    {
        return UsefullMethods.highlightString(Percent.ToString() + "%")
            + " bigger Weapon ";
    }
    public override string title()
    {
        return "WRIST";
    }
}
