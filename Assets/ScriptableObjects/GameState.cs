using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameState")]
public class GameState : ScriptableObject
{
    [Serializable]
    public class BossAreaDoor
    {
        public bool isCompleted;
        public DoorAnimationController DoorController;
    }
    public BossAreaDoor[] FourDoors;
    public int LastEnteredDoor;
    public int LastCompletedBoss;
    public bool isFinalDoorOpen;
    public bool isTutorialComplete;
    public bool isSpawnWithouUpgrades;
    public bool justDefeatedBoss;

    public List<Upgrade> playerUpgrades = new List<Upgrade>();

    public Upgrade lastLostUpgrade;
    public Vector3 IndexOfLostUpgradeRoom; // X is the Group, Y is the Room, Z is the Area
    public bool isLostUpgradeAvailable;

    public List<Room_script> currentPlayersRooms = new List<Room_script>();
    public List<Vector3Int> currentPlayerRooms_index = new List<Vector3Int>();

    public int IndexOfCurrentWeapon;

    [Serializable]
    public class weaponInfos
    {
        public GameObject weaponPrefab;
        public bool isUnlocked;
        //public bool isCurrent;
    }
    public List<weaponInfos> WeaponInfosList = new List<weaponInfos>();


    public int[] FurthestDoorsArray = new int[5];

    public int actuallyUnlockedSkulls;
    public int SkullsThatShouldBeUnlocked;
    public int finalDoor_DialogueIndex;

    public bool hasPickedFirstUpgrade;
    public bool hasPickedFirstWeapon;

    [Header("Audio")]
    [Range(0, 1)] public float MusicVolum;
    [Range(0, 1)] public float SFXVolum;

    [Header("Levels and XP")]
    public int level;
    public float level_currentMaxHp;
    public float level_currentDamge;
    public float level_currentMaxStamina;

    
    int xpPoints;
    public Action<int> OnXpPointsSet;
    public int XpPoints 
    {
        get { return xpPoints; }
        set
        {
            if(xpPoints == value) { return; }
            xpPoints = value;
            OnXpPointsSet?.Invoke(value);
        }
    }
    
    public void ResetState()
    {
        foreach (BossAreaDoor bossAreaDoor in FourDoors)
        {
            bossAreaDoor.isCompleted = false;
        }
        isFinalDoorOpen = false;
        isTutorialComplete = false;
        justDefeatedBoss = false;

        LastEnteredDoor = -1;

        playerUpgrades.Clear();

        lastLostUpgrade = null;

        IndexOfCurrentWeapon = 0;

        for (int i = 0; i < WeaponInfosList.Count; i++)
        {
            if(i == 0) 
            { 
                WeaponInfosList[i].isUnlocked = true;
            }
            else
            {
                WeaponInfosList[i].isUnlocked = false;
            }
        }

        for (int i = 0; i < FurthestDoorsArray.Length; i++)
        {
            FurthestDoorsArray[i] = -1;
        }

        actuallyUnlockedSkulls = 0;
        SkullsThatShouldBeUnlocked = 0;
        finalDoor_DialogueIndex = 0;

        hasPickedFirstUpgrade = false;
        hasPickedFirstWeapon = false;

        XpPoints = 0;

    }
}
