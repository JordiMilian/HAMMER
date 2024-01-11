using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    [Serializable]
    public class Room
    {
        public DoorController doorController;
        public Generic_HealthSystem[] Enemies;
        public int EnemiesAlive;

        public void EnemyDied(object sender, EventArgs args)
        {
            EnemiesAlive--;
            if(EnemiesAlive <= 0)
            {
                OpenDoor();
            }
        }
        public void OpenDoor()
        {
            doorController.OpenDoor();
        }

    }
    
    public Room[] battleRooms = new Room[0];
    private void OnEnable()
    {
        foreach (Room room in battleRooms)
        {
            foreach (Generic_HealthSystem enemyHealth in room.Enemies)
            {
                enemyHealth.OnDeath += room.EnemyDied;
            }
        }
    }
    private void OnDisable()
    {
        foreach (Room room in battleRooms)
        {
            foreach (Generic_HealthSystem enemyHealth in room.Enemies)
            {
                enemyHealth.OnDeath -= room.EnemyDied;
            }
        }
    }
    private void Start()
    {
        foreach(Room room in battleRooms)
        {
            room.EnemiesAlive = room.Enemies.Length;
            if (room.EnemiesAlive <= 0) room.OpenDoor();

        }
    }
}
