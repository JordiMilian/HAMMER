using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades_PickableUpgrades : MonoBehaviour
{
   public List<GameObject> AvailableUpgrades;
    public static Upgrades_PickableUpgrades Instance;
    private void Awake()
    {
        //Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public GameObject GetRandomUpgrade()
    {
        int randomIndex = Random.Range(0, AvailableUpgrades.Count);
        return AvailableUpgrades[randomIndex];
    }
}
