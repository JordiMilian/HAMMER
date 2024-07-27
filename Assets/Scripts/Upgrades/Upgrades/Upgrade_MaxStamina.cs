using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Max Stamina", fileName = "Max Stamina")]
public class Upgrade_MaxStamina : Upgrade
{
    FloatVariable playerMaxStamina;
    FloatVariable baseStamina;
    [SerializeField] float Percent;
    public override void onAdded(GameObject entity)
    {
        Player_References refs = entity.GetComponent<Player_References>();

        baseStamina = refs.baseStamina;
        playerMaxStamina = refs.maxStamina;

        float addedStamina = baseStamina.GetValue() * UsefullMethods.normalizePercentage(Percent, false, true);
        playerMaxStamina.SetValue(playerMaxStamina.GetValue() + addedStamina);
    }
    public override void onRemoved(GameObject entity)
    {
        float removedStamina = baseStamina.GetValue() * UsefullMethods.normalizePercentage(Percent, false, true);
        playerMaxStamina.SetValue(playerMaxStamina.GetValue() - removedStamina);
    }
    public override string shortDescription()
    {
        return UsefullMethods.highlightString(Percent.ToString() + "%")
            + " more Stamina";
    }
}
