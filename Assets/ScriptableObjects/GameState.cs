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
    public bool isTutorialCompleted;

    public List<Upgrade> playerUpgrades = new List<Upgrade>();

    public Upgrade lastLostUpgrade;

    public List<Room_script> currentPlayersRooms = new List<Room_script>();
}
