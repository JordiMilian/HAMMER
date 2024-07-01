using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class Upgrades_ScriptsHolder : Upgrade
{
    public override void onAdded(GameObject entity)
    {

    }
    public override void onRemoved(GameObject entity)
    {
        
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
