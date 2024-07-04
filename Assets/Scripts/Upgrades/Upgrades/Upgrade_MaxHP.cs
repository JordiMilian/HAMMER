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
        float newValue = playerHealth.MaxHP.GetValue() * (1 + (Percent / 100));
        playerHealth.MaxHP.SetValue(newValue);
    }
    public override void onRemoved(GameObject entity)
    {
        float newValue = playerHealth.MaxHP.GetValue() / (1 + (Percent / 100));
        playerHealth.MaxHP.SetValue(newValue);
    }
    public override string shortDescription()
    {
        return "<color=red>" + Percent + "%<color=black> more HP";
    }
}
