using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "GameState")]
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

    public GameObject PlayersWeaponPrefab;
    public List<GameObject> weaponsPrefabList = new List<GameObject>();

    public int[] FurthestDoorsArray = new int[5];

    public int actuallyUnlockedSkulls;
    public int SkullsThatShouldBeUnlocked;
    public int finalDoor_DialogueIndex;

    public bool hasPickedFirstUpgrade;
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

        PlayersWeaponPrefab = weaponsPrefabList[0];

        for (int i = 0; i < FurthestDoorsArray.Length; i++)
        {
            FurthestDoorsArray[i] = -1;
        }

        actuallyUnlockedSkulls = 0;
        SkullsThatShouldBeUnlocked = 0;
        finalDoor_DialogueIndex = 0;

        hasPickedFirstUpgrade = false;

    }
}
