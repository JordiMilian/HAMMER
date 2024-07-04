using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Max Stamina", fileName = "Max Stamina")]
public class Upgrade_MaxStamina : Upgrade
{
    FloatVariable playerMaxStamina;
    FloatVariable playerCurrentStamina;
    [SerializeField] float Percent;
    public override void onAdded(GameObject entity)
    {
        playerMaxStamina = entity.GetComponent<Player_References>().maxStamina;
        playerMaxStamina.SetValue(playerMaxStamina.GetValue() * (1 + (Percent / 100)));
    }
    public override void onRemoved(GameObject entity)
    {
        playerMaxStamina.SetValue(playerMaxStamina.GetValue() / (1 + (Percent / 100)));
    }
    public override string shortDescription()
    {
        return UsefullMethods.highlightString(Percent.ToString() + "%")
            + " more Stamina";
    }
}
