using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Max Stamina", fileName = "Max Stamina")]
public class Upgrade_MaxStamina : Upgrade
{
    FloatVariable playerStamina;
    [SerializeField] float Percent;
    public override void onAdded(GameObject entity)
    {
        playerStamina = entity.GetComponent<Player_References>().maxStamina;
        playerStamina.SetValue(playerStamina.GetValue() * (1 + (Percent / 100)));
    }
    public override void onRemoved(GameObject entity)
    {
        playerStamina.SetValue(playerStamina.GetValue() / (1 + (Percent / 100)));
    }
}
