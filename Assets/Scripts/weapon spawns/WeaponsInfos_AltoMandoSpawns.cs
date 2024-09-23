using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsInfos_AltoMandoSpawns : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [Serializable]
    public class weaponSpawner
    {
        public bool isSpawned;
        public Transform SpawnPositionTf;
    }
    public List<weaponSpawner> weaponSpawnersList = new List<weaponSpawner> ();
    private void OnEnable()
    {
        checkUnlockedWeapons();
        GlobalPlayerReferences.Instance.references.events.OnPickedNewWeapon += OnPickedAnyWeapon;
    }
    void checkUnlockedWeapons()
    {
        for (int i = 0; i < gameState.WeaponInfosList.Count; i++)
        {
            if (gameState.WeaponInfosList[i].isUnlocked && gameState.IndexOfCurrentWeapon != i) //if its unlocked and its not the current weapon
            {
                if (!weaponSpawnersList[i].isSpawned) //if its not already spawned
                {
                    GameObject InstantiatedWeapon = Instantiate(
                        gameState.WeaponInfosList[i].weaponPrefab,
                        weaponSpawnersList[i].SpawnPositionTf.position,
                        Quaternion.identity,
                        transform
                        );

                    weaponSpawnersList[i].isSpawned = true;

                    //InstantiatedWeapon.GetComponent<WeaponPrefab_infoHolder>().OnPickedUpEvent += OnPickedAnyWeapon;
                }
            }
        }
    }
    void OnPickedAnyWeapon(WeaponPrefab_infoHolder infoHolder)
    {
        weaponSpawnersList[infoHolder.indexInGameState].isSpawned = false;
        checkUnlockedWeapons();
    }
}
