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
        float percentMultiplier = 1 + (Percent / 100);
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
