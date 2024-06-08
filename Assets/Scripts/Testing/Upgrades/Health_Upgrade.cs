using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Upgrades/Health Upgrade")] 
public class Health_Upgrade : Upgrade
{
    [SerializeField] float percentage;
    public override void onAdded(GameObject entity)
    {
        entity.GetComponent<Testing_PlayerStats>().Health *= (1 + (percentage/100));
    }
    public override void onRemoved(GameObject entity)
    {
        entity.GetComponent<Testing_PlayerStats>().Health /= (1 + (percentage/100));
    }
}
