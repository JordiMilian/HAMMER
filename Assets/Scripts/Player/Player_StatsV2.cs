using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StatsV2 : MonoBehaviour
{
    //THis script is to hold the Stats ONLY.
    //Do not put logic of picking up upgrades or noseque. All of those should be called from the StatsController
    public enum statTypes
    {
        MaxHealth,MaxStamina,DamageMultiplier
    }
    [Serializable]
    public class Stat
    {
        public statTypes type;
        public int BasePoints { get; private set; }
        public int UpgradePoints { get; private set; }
        public float valueAtZero;
        public float valuePerPoint;
        public float GetCurrentValue()
        {
            return valueAtZero + ((BasePoints + UpgradePoints) * valuePerPoint);
        }
        public void ResetBasePoints() { BasePoints = 0; }
        public void AddBasePoint() { BasePoints++; }
        public void ResetUpgradePoints() { UpgradePoints = 0; }
        public void AddUpgradePoint() {  UpgradePoints++; }
    }
    [SerializeField] List<Stat> statsList = new List<Stat>();
    [SerializeField] int PointsToRandomize;

    public Stat GetStatByType(statTypes type)
    {
        foreach (Stat stat in statsList)
        {
            if(stat.type == type) return stat;
        }
        Debug.LogError("Stat type "+type+" not available in List");
        return null;
    }

    public void RandomizeAllBasePoints()
    {
        foreach (Stat stat in statsList)
        {
            stat.ResetBasePoints();
        }

        for (int p = 0; p < PointsToRandomize; p++)
        {
            int randomIndex = UnityEngine.Random.Range(0, statsList.Count - 1);
            statsList[randomIndex].AddBasePoint();
        }
    }
}
