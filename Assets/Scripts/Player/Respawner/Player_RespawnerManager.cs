using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_RespawnerManager : MonoBehaviour
{

    public List<Player_Respawner> Respawners = new List<Player_Respawner>();
    [SerializeField] Player_Respawner CurrentFurthestRespawner;
    //[SerializeField] GameObject PlayerGO;
    [SerializeField] Player_EventSystem eventSystem;
    

    private void OnEnable()
    {
        
        eventSystem.CallRespawnToLastRespawner += RespawnPlayer;
        foreach (Player_Respawner respawner in Respawners)
        {
            respawner.OnRespawnerActivated += CheckFurthestRespawner;
        }
    }
    private void OnDisable()
    {
        eventSystem.CallRespawnToLastRespawner -= RespawnPlayer;
        foreach (Player_Respawner respawner in Respawners)
        {
            respawner.OnRespawnerActivated -= CheckFurthestRespawner;
        }
    }
    public static Player_RespawnerManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void RespawnPlayer()
    {
        CheckFurthestRespawner();
        CurrentFurthestRespawner.gameObject.GetComponent<TiedEnemy_StateMachine>().ShowBodies();
        CurrentFurthestRespawner.RespawnFromHere(eventSystem.gameObject); //Go to Player_Respawn
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
            Respawners[i].distanceToManager = (Respawners[i].transform.position - transform.position).sqrMagnitude;
            if(Respawners[i].distanceToManager > maxDistance && Respawners[i].IsActivated)
            {
                furthestIndex = i;
                maxDistance = Respawners[i].distanceToManager;
            }
        }
        return Respawners[furthestIndex];
    }
}
