using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_UpgradesManager : MonoBehaviour
{
    [SerializeField] bool trigger_DeleteRandomUpgrade;
    public List<Upgrade> UpgradesList = new List<Upgrade>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UpgradeContainer upgradeContainer = collision.GetComponent<UpgradeContainer>();
        if (upgradeContainer != null)
        {
            Upgrade upgradeEffector = upgradeContainer.upgradeEffect;
            upgradeEffector.onAdded(gameObject); //aply the effect of the upgrade
            UpgradesList.Add(upgradeEffector); //add the effector to a List
            Destroy(collision.gameObject); //destroy the other object
        }
    }
    void deleteRandomUpgrade()
    {
        if(UpgradesList.Count == 0) { Debug.Log("No upgrades to delete"); return;}
        int randomIndex = Random.Range(0,UpgradesList.Count-1);
        deleteUpgrade(randomIndex);

    }
    void deleteUpgrade(int i)
    {
        UpgradesList[i].onRemoved(gameObject);
        UpgradesList.RemoveAt(i);
    }
    private void Update()
    {
       if(trigger_DeleteRandomUpgrade)
        {
            deleteRandomUpgrade();
            trigger_DeleteRandomUpgrade = false;
        }
    }
}
