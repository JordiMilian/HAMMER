using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_RespawnerManager : MonoBehaviour
{

    public List<Player_Respawner> Respawners = new List<Player_Respawner>();
    [SerializeField] Player_Respawner CurrentFurthestRespawner;
    [SerializeField] GameObject PlayerGO;
    Generic_HealthSystem playerHealth;
    [SerializeField] Player_EventSystem eventSystem;
    

    
    private void OnEnable()
    {
        playerHealth = PlayerGO.GetComponent<Generic_HealthSystem>();
        eventSystem.CallRespawn += RespawnPlayer;
        foreach (Player_Respawner respawner in Respawners)
        {
            respawner.OnRespawnerActivated += CheckFurthestRespawner;
        }
    }
    private void OnDisable()
    {
        eventSystem.CallRespawn -= RespawnPlayer;
        foreach (Player_Respawner respawner in Respawners)
        {
            respawner.OnRespawnerActivated -= CheckFurthestRespawner;
        }
    }
    public void RespawnPlayer()
    {
        CheckFurthestRespawner();
        CurrentFurthestRespawner.gameObject.GetComponent<TiedEnemy_StateMachine>().ShowBodies();
        CurrentFurthestRespawner.RespawnFromHere(PlayerGO); //Go to Player_Respawn
        playerHealth.RestoreAllHealth();
    }
    void CheckFurthestRespawner()
    {
        CurrentFurthestRespawner = FindFurthestActiveRespawner();
    }
    Player_Respawner FindFurthestActiveRespawner()
    {
        int furthestIndex = 0;
        float maxDistance = 0;
        for(int i = 0;i<Respawners.Count;i++)
        {
            //Sorting by distance to the Manger gameobject
            Respawners[i].distanceToManager = (Respawners[i].transform.position - transform.position).magnitude;
            if(Respawners[i].distanceToManager > maxDistance && Respawners[i].IsActivated)
            {
                furthestIndex = i;
                maxDistance = Respawners[i].distanceToManager;
            }
        }
        return Respawners[furthestIndex];
    }
}
