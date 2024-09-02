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
    public int LastCompletedIndex;
    public bool isFinalDoorOpen;
    public bool isTutorialComplete;
    public bool isSpawnWithouUpgrades;

    public List<Upgrade> playerUpgrades = new List<Upgrade>();

    public Upgrade lastLostUpgrade;
    public Vector3 IndexOfLostUpgradeRoom; // X is the Group, Y is the Room, Z is the Area
    public bool isLostUpgradeAvailable;

    public List<Room_script> currentPlayersRooms = new List<Room_script>();
    public List<Vector3> currentPlayerRooms_index = new List<Vector3>();

    public GameObject PlayersWeaponPrefab;
    public List<GameObject> weaponsPrefabList = new List<GameObject>();
    public void ResetState()
    {
        foreach (BossAreaDoor bossAreaDoor in FourDoors)
        {
            bossAreaDoor.isCompleted = false;
        }
        isFinalDoorOpen = false;

        LastCompletedIndex = -1;

        playerUpgrades.Clear();

        lastLostUpgrade = null;
    }
}
