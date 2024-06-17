using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_UpgradesManager : MonoBehaviour
{
    [SerializeField] bool trigger_DeleteRandomUpgrade;
    [SerializeField] GameState gameState;
    [SerializeField] Player_EventSystem playerEvents;
    private void Awake()
    {
        foreach (Upgrade upgrade in gameState.playerUpgrades)
        {
            upgrade.onAdded(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UpgradeContainer upgradeContainer = collision.GetComponent<UpgradeContainer>(); //CREA UN TAG O ALGUNA COSA PERFA
        if (upgradeContainer != null)
        {
           AddNewUpgrade( upgradeContainer.upgradeEffect);
            playerEvents.OnPickedNewUpgrade?.Invoke(upgradeContainer);
        }
    }
    void AddNewUpgrade(Upgrade upgrade)
    {
        upgrade.onAdded(gameObject);
        gameState.playerUpgrades.Add(upgrade);
        
    }
    void deleteRandomUpgrade()
    {
        if(gameState.playerUpgrades.Count == 0) { Debug.Log("No upgrades to delete"); return;}
        int randomIndex = Random.Range(0, gameState.playerUpgrades.Count-1);
        deleteUpgrade(randomIndex);

    }
    void deleteUpgrade(int i)
    {
        gameState.playerUpgrades[i].onRemoved(gameObject);
        gameState.playerUpgrades.RemoveAt(i);
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
