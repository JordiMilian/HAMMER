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
    [SerializeField] Generic_OnTriggerEnterEvents nearbyUpgradesCollider;
    public Action OnUpdatedUpgrades;
    [SerializeField] AudioClip SFX_PickedUpgrade;

    private void OnEnable()
    {
        InputDetector.Instance.OnSelectPressed += PickUpNearestUpgrade;
        nearbyUpgradesCollider.AddActivatorTag(Tags.UpgradeContainer);
        nearbyUpgradesCollider.OnTriggerEntered += onEnteredNearUpgrade;
        nearbyUpgradesCollider.OnTriggerExited += onExitedNearUpgrade;

        foreach (Upgrade upgrade in gameState.playerUpgrades)
        {
            upgrade.onAdded(gameObject);
        }
    }
    private void OnDisable()
    {
        InputDetector.Instance.OnSelectPressed -= PickUpNearestUpgrade;
        UpgradeColliderDetector.OnTriggerEntered -= onSingleTriggerEnter;
        nearbyUpgradesCollider.OnTriggerExited -= onExitedNearUpgrade;

        foreach (Upgrade upgrade in gameState.playerUpgrades)
        {
            upgrade.onRemoved(gameObject);
        }
    }
    [SerializeField] List<UpgradeContainer> upgradesNear = new();
    private void onEnteredNearUpgrade(Collider2D collider)
    {
        if(collider.TryGetComponent(out UpgradeContainer container))
        {
            upgradesNear.Add(container);
        }
    }
    private void onExitedNearUpgrade(Collider2D collider)
    {
        if (collider.TryGetComponent(out UpgradeContainer container))
        {
            upgradesNear.Remove(container);
        }
    }
    private void PickUpNearestUpgrade()
    {
        if(upgradesNear.Count == 0) { return; }

        float nearestDistance = 999;
        int nearestIndex = -1;
        for (int i = 0; i < upgradesNear.Count; i++)
        {
            float thisDistance = (transform.position - upgradesNear[i].transform.position).sqrMagnitude;
            if (thisDistance < nearestDistance)
            {
                nearestDistance = thisDistance;
                nearestIndex = i;
            }
        }
        UpgradeContainer upgradeContainer = upgradesNear[nearestIndex];
        upgradeContainer.OnPickedUpContainer();
        AddNewUpgrade(upgradeContainer.upgradeEffect);

        //Audio and Visual feedback (should we make a new state?)
        SFX_PlayerSingleton.Instance.playSFX(SFX_PickedUpgrade);
        CameraShake.Instance.ShakeCamera(IntensitiesEnum.Small);
        TimeScaleEditor.Instance.HitStop(IntensitiesEnum.VerySmall);
        playerRefs.flasher.CallDefaultFlasher();
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
                
            }
           
        }
    }
    void AddNewUpgrade(Upgrade upgrade)
    {
        upgrade.onAdded(playerRefs.gameObject);
        OnUpdatedUpgrades?.Invoke();
        gameState.playerUpgrades.Add(upgrade);
        
    }
    void deleteUpgrade(int i)
    {
        gameState.lastLostUpgrade = gameState.playerUpgrades[i];


        gameState.playerUpgrades[i].onRemoved(gameObject); //remove effect
        gameState.playerUpgrades.RemoveAt(i); //remove from list
        OnUpdatedUpgrades?.Invoke();
    }
}
