using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_UpgradesManager : MonoBehaviour
{
    [SerializeField] bool trigger_DeleteRandomUpgrade;
    [SerializeField] GameState gameState;
    [SerializeField] Player_References playerRefs;
    [SerializeField] Generic_OnTriggerEnterEvents UpgradeColliderDetector;
    public Action OnUpdatedUpgrades;
    [SerializeField] AudioClip SFX_PickedUpgrade;

    private void OnEnable()
    {
        UpgradeColliderDetector.AddActivatorTag(Tags.UpgradeContainer);
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
        if(collision.CompareTag(Tags.UpgradeContainer))
        {
            
            UpgradeContainer upgradeContainer = collision.GetComponent<UpgradeContainer>(); //CREA UN TAG O ALGUNA COSA PERFA
            if(upgradeContainer != null)
            {
                Debug.Log("upgrade:" + collision.name);
                upgradeContainer.OnPickedUpContainer();
                AddNewUpgrade(upgradeContainer.upgradeEffect);

                //Audio and Visual feedback (should we make a new state?)
                SFX_PlayerSingleton.Instance.playSFX(SFX_PickedUpgrade);
                CameraShake.Instance.ShakeCamera(IntensitiesEnum.Small);
                TimeScaleEditor.Instance.HitStop(IntensitiesEnum.VerySmall);
                playerRefs.flasher.CallDefaultFlasher();

                //event just in case (UI?)
                OnUpdatedUpgrades?.Invoke();
            }
           
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
        int randomIndex = UnityEngine.Random.Range(0, gameState.playerUpgrades.Count-1);
        deleteUpgrade(randomIndex);

    }
    void deleteUpgrade(int i)
    {
        gameState.lastLostUpgrade = gameState.playerUpgrades[i];


        gameState.playerUpgrades[i].onRemoved(gameObject); //remove effect
        gameState.playerUpgrades.RemoveAt(i); //remove from list
        OnUpdatedUpgrades?.Invoke();
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
