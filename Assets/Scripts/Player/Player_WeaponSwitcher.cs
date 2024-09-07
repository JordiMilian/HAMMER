using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WeaponSwitcher : MonoBehaviour
{
    /*
    How to make a new weapon:
    -Prefabs-weaponInfoHolder-Duplicate a new infoHolder
    -In the script change the "Weapon Sprite" and setup the values for damages
    -Go to Animations files and duplicate a players animator, for example "MainCharacter_PinkWeapon"
    -Go the the players prefab and manually change the Animator, Sprite and Collider
    -Copy the Colliders properties and paste them in the infoHolder collider
    -Set up the animations and animator as you want
    -Add the prefab to the gameState and set the indexInGameState of InfoHolder to where its in the list

    */
    [SerializeField] GameState gameState;
    [SerializeField] PolygonCollider2D playersWeaponCollider;
    [SerializeField] SpriteRenderer playersWeaponSpriteRenderer;

    [SerializeField] Generic_OnTriggerEnterEvents weaponPickerDetector;
    [SerializeField] Player_References playerRefs;
    private void OnEnable()
    {
        weaponPickerDetector.AddActivatorTag(TagsCollection.Pickeable);

        weaponPickerDetector.OnTriggerEntered += OnFoundPickeable;

    }
    private void OnDisable()
    {
        weaponPickerDetector.RemoveActivatorTag(TagsCollection.Pickeable);
        weaponPickerDetector.OnTriggerEntered -= OnFoundPickeable;
    }
    void OnFoundPickeable(Collider2D collider)
    {
        WeaponPrefab_infoHolder infoHolder = collider.GetComponent<WeaponPrefab_infoHolder>();

        if (infoHolder != null)
        {
            gameState.PlayersWeaponPrefab = gameState.weaponsPrefabList[infoHolder.indexInGameState];
            //SetGameStateWeapon(infoHolder.ownPrefab);
            SetNewWeapon(collider.gameObject);
            playerRefs.events.OnPickedNewWeapon?.Invoke(infoHolder);
            infoHolder.OnPickedUp();
        }
    }
    void SetGameStateWeapon(GameObject weapon)
    {
        foreach(GameObject weaponPrefab in gameState.weaponsPrefabList)
        {
            if(weaponPrefab == weapon)
            {
                Debug.Log("it is the same");
                gameState.PlayersWeaponPrefab = weaponPrefab;
                return;
            }
        }
    }
    private void Awake()
    {
        SetNewWeapon(gameState.PlayersWeaponPrefab);
    }
    public void SetNewWeapon(GameObject newWeaponPrefab)
    {
        WeaponPrefab_infoHolder infoHolder = newWeaponPrefab.GetComponent<WeaponPrefab_infoHolder>();

        CopyPolygonCollider(infoHolder.weaponsCollider, playersWeaponCollider);

        playerRefs.comboSystem.Base_Damage = infoHolder.BaseDamage;
        playerRefs.comboSystem.Base_Knockback = infoHolder.BaseKnockback;
        playerRefs.comboSystem.Base_HitStop = infoHolder.BaseHitstop;

        playerRefs.specialAttack.Sp_Damage = infoHolder.Sp_Damage;
        playerRefs.specialAttack.Sp_Knockback = infoHolder.Sp_Knockback;
        playerRefs.specialAttack.StaminaCost = infoHolder.Sp_StaminaCost;

        playerRefs.comboSystem.StaminaUse = infoHolder.StaminaUsePerSwing;

        playerRefs.animator.runtimeAnimatorController = infoHolder.animatorController;

        playersWeaponSpriteRenderer.sprite = infoHolder.weaponSprite;
    }
    void CopyPolygonCollider(PolygonCollider2D sourceCollider, PolygonCollider2D targetCollider)
    {
        targetCollider.pathCount = sourceCollider.pathCount;
        for (int i = 0; i < sourceCollider.pathCount; i++)
        {
            targetCollider.SetPath(i, sourceCollider.GetPath(i));
        }
        targetCollider.offset = sourceCollider.offset;
        targetCollider.isTrigger = sourceCollider.isTrigger;
    }
}