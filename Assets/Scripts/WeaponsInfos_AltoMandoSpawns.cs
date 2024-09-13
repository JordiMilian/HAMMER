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
        public bool isUnlocked;
        public bool isCurrentWeapon;
        public int areaIndex; //Weapons with areaIndex >0 are asigned to an Area door. <0 are asigned as you wish
        public GameObject infoHolderPrefab;
        public GameObject spawnedInstance;
        public Transform SpawnPositionTf;
        [HideInInspector] public WeaponPrefab_infoHolder infoHolder;
    }
    public List<weaponSpawner> weaponSpawners = new List<weaponSpawner> ();
    private void Awake()
    {
        checkUnlockedAreasOnAwake();
    }
    void checkUnlockedAreasOnAwake()
    {
        for (int i = 0; i < weaponSpawners.Count; i++)
        {
            if(weaponSpawners[i].areaIndex < 0) { continue; }

            if (gameState.FourDoors[weaponSpawners[i].areaIndex].isCompleted)
            {
                weaponSpawners[i].isUnlocked = true;
            }
        }
        checkCurrentAndSpawn();
    }
    void SpawnWeapons()
    {
        for (int i = 0; i < weaponSpawners.Count; i++)
        {
            //Debug.Log("is it unlocked?");
            if (!weaponSpawners[i].isUnlocked) { continue; }

            //Debug.Log("is it current?");
            if (weaponSpawners[i].isCurrentWeapon)
            {
                //Algo aqui pa marcar que ja la tens desploquejada pero la tens en mans
                continue;
            }
            //Debug.Log("is it null?");
            if (weaponSpawners[i].spawnedInstance == null)
            {
                //Debug.Log("spawned new weapon");
                weaponSpawners[i].spawnedInstance = spawnWeapon(weaponSpawners[i]);
                weaponSpawners[i].infoHolder = weaponSpawners[i].spawnedInstance.GetComponent<WeaponPrefab_infoHolder>();

                weaponSpawners[i].infoHolder.OnPickedUpEvent += checkCurrentAndSpawn;
            }
        }
    }
    GameObject spawnWeapon(weaponSpawner spawner)
    {
        return Instantiate(spawner.infoHolderPrefab, spawner.SpawnPositionTf.position, Quaternion.identity);
    }
    void checkCurrentAndSpawn()
    {
        checkCurrentWeapon();
        SpawnWeapons();
    }
    void checkCurrentWeapon()
    {
        foreach(weaponSpawner spawner in weaponSpawners)
        {
            spawner.isCurrentWeapon = spawner.infoHolderPrefab == gameState.PlayersWeaponPrefab;
        }
    }
}
