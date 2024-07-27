using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Weapon Size")]
public class Upgrade_WeaponSizee : Upgrade
{
    [SerializeField] float Percent;
    GameObject playerWeapon;
    public override void onAdded(GameObject entity)
    {
        playerWeapon = entity.GetComponent<Player_References>().weaponScalingRoot;

        float addedScale = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
        playerWeapon.transform.localScale = new Vector3(
            playerWeapon.transform.localScale.x + addedScale,
            playerWeapon.transform.localScale.y + addedScale,
            playerWeapon.transform.localScale.z
            );
    }
    public override void onRemoved(GameObject entity)
    {

        float addedScale = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
        playerWeapon.transform.localScale = new Vector3(
            playerWeapon.transform.localScale.x - addedScale,
            playerWeapon.transform.localScale.y - addedScale,
            playerWeapon.transform.localScale.z
            );
    }
    public override string shortDescription()
    {
        return UsefullMethods.highlightString(Percent.ToString() + "%")
            + " bigger Weapon ";
    }
}
