using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    -Go to AltoMando_Room prefab, find weaponInfo_Spawns and add a new element. Make sure the areaIndex is the proper area


    */
    public Action<int> OnPickedNewWeapon; //passes the index in gamestate?

    [SerializeField] GameState gameState;
    [SerializeField] PolygonCollider2D playersWeaponCollider;
    [SerializeField] SpriteRenderer playersWeaponSpriteRenderer;

    [SerializeField] Generic_OnTriggerEnterEvents weaponPickerDetector;
    [SerializeField] Player_References playerRefs;
    [SerializeField] AudioClip SFX_FoundWeapon;
    private void OnEnable()
    {
        weaponPickerDetector.AddActivatorTag(Tags.Pickeable);

        weaponPickerDetector.OnTriggerEntered += OnFoundPickeable;

    }
    private void OnDisable()
    {
        weaponPickerDetector.RemoveActivatorTag(Tags.Pickeable);
        weaponPickerDetector.OnTriggerEntered -= OnFoundPickeable;
    }
    void OnFoundPickeable(Collider2D collider)
    {
        WeaponPickable_Controller WeaponPickable = collider.GetComponent<WeaponPickable_Controller>();


        //The pickable prefab only hold an index. This index is a reference to the List of all weapons in the gameState
        if (WeaponPickable != null)
        {
            WeaponPickable.OnPickedUp();//Maybe this should be in the prefab Controller?

            int indexInGameState = WeaponPickable.indexInGameState;
            GameObject WeaponStatesPrefab = gameState.WeaponInfosList[indexInGameState].weaponStatesHolderPrefab;
            gameState.WeaponInfosList[indexInGameState].isUnlocked = true;
            gameState.IndexOfCurrentWeapon = indexInGameState;
            SetNewWeapon(WeaponStatesPrefab);


            //Audio and Visual Feedback here
            PickedWeaponFeedback();

            OnPickedNewWeapon?.Invoke(indexInGameState);
        }
    }
    void PickedWeaponFeedback()
    {
        CameraShake.Instance.ShakeCamera(.3f, 0.1f);
        TimeScaleEditor.Instance.HitStop(0.05f);
        playerRefs.flasher.CallDefaultFlasher();

        SFX_PlayerSingleton.Instance.playSFX(SFX_FoundWeapon);
    }
    private void Awake()
    {
        SetNewWeapon(gameState.WeaponInfosList[gameState.IndexOfCurrentWeapon].weaponStatesHolderPrefab);
    }
    public void SetNewWeapon(GameObject weaponPrefab)
    {
        if (playerRefs.WeaponStatesHolder != null) { Destroy(playerRefs.WeaponStatesHolder); }

        playerRefs.WeaponStatesHolder = Instantiate(weaponPrefab, playerRefs.StatesRoots);
        Weapon_InfoHolder thisInfoHolder = playerRefs.WeaponStatesHolder.GetComponent<Weapon_InfoHolder>();
        playerRefs.RollingAttackState = thisInfoHolder.RollAttack;
        playerRefs.ParryAttackState = thisInfoHolder.ParryAttack;
        playerRefs.SpecialAttackState = thisInfoHolder.SpecialAttack;
        playerRefs.StartingComboAttackState = thisInfoHolder.ComboAttacksList[0];
        //Collider and sprite
        playersWeaponSpriteRenderer.sprite = thisInfoHolder.WeaponSprite;
        CopyPolygonCollider(thisInfoHolder.WeaponCollider, playersWeaponCollider);
    }
    static void CopyPolygonCollider(PolygonCollider2D sourceCollider, PolygonCollider2D targetCollider)
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
