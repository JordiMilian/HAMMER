using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_UpgradesManager : MonoBehaviour
{
    [SerializeField] bool trigger_DeleteRandomUpgrade;
    [SerializeField] GameState gameState;
    [SerializeField] Player_EventSystem playerEvents;
    [SerializeField] Generic_OnTriggerEnterEvents UpgradeColliderDetector;
    private void OnEnable()
    {
        UpgradeColliderDetector.AddActivatorTag(TagsCollection.UpgradeContainer);
        UpgradeColliderDetector.OnTriggerEntered += onSingleTriggerEnter;

        foreach (Upgrade upgrade in gameState.playerUpgrades)
        {
            upgrade.onAdded(gameObject);
        }
    }
    private void OnDisable()
    {
        UpgradeColliderDetector.OnTriggerEntered -= onSingleTriggerEnter;

        foreach (Upgrade upgrade in gameState.playerUpgrades)
        {
            upgrade.onRemoved(gameObject);
        }
    }
    private void onSingleTriggerEnter(Collider2D collision)
    {
        if(collision.CompareTag(TagsCollection.UpgradeContainer))
        {
            Debug.Log("upgrade:" +  collision.name);
            UpgradeContainer upgradeContainer = collision.GetComponent<UpgradeContainer>(); //CREA UN TAG O ALGUNA COSA PERFA
            upgradeContainer.OnPickedUpContainer();
            AddNewUpgrade(upgradeContainer.upgradeEffect);
            playerEvents.OnPickedNewUpgrade?.Invoke(upgradeContainer);
        }
    }
    void AddNewUpgrade(Upgrade upgrade)
    {
        upgrade.onAdded(gameObject);
        gameState.playerUpgrades.Add(upgrade);
        
    }
    public void deleteRandomUpgrade()
    {
        if(gameState.playerUpgrades.Count == 0) { Debug.Log("No upgrades to delete"); return;}
        int randomIndex = Random.Range(0, gameState.playerUpgrades.Count-1);
        deleteUpgrade(randomIndex);

    }
    void deleteUpgrade(int i)
    {
        gameState.lastLostUpgrade = gameState.playerUpgrades[i];


        gameState.playerUpgrades[i].onRemoved(gameObject); //remove effect
        gameState.playerUpgrades.RemoveAt(i); //remove from list
        playerEvents.OnRemovedUpgrade?.Invoke(); //Call the event
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
