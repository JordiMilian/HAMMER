using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Max HP")]
public class Upgrade_MaxHP : Upgrade
{
    [SerializeField] float Percent;
    Player_HealthSystem playerHealth;
    public override void onAdded(GameObject entity)
    {
        playerHealth = entity.GetComponent<Player_HealthSystem>();

        float addedValue = (playerHealth.BaseHP.GetValue() * UsefullMethods.normalizePercentage(Percent,false,true));
        playerHealth.MaxHP.ChangeValue(playerHealth.MaxHP.GetValue() + addedValue);
        playerHealth.CurrentHP.ChangeValue(playerHealth.CurrentHP.GetValue() + addedValue);

    }
    public override void onRemoved(GameObject entity)
    {
        float removedValue = (playerHealth.BaseHP.GetValue() * UsefullMethods.normalizePercentage(Percent,false, true));
        //float newValue = playerHealth.MaxHP.GetValue() / (1 + (Percent / 100));
        playerHealth.MaxHP.ChangeValue(playerHealth.MaxHP.GetValue() - removedValue);
        playerHealth.CurrentHP.ChangeValue (playerHealth.CurrentHP.GetValue() - removedValue);
    }
    public override string shortDescription()
    {
        return "<color=red>" + Percent + "%<color=black> more HP";
    }
}
