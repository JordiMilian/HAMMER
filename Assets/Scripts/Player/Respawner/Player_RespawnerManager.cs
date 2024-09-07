using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_RespawnerManager : MonoBehaviour
{

    public List<Player_Respawner> Respawners = new List<Player_Respawner>();
    bool isSorted;
    [SerializeField] Player_Respawner CurrentFurthestRespawner;
    //[SerializeField] GameObject PlayerGO;
    [SerializeField] Player_EventSystem eventSystem;
    [SerializeField] GameState gameState;
    [SerializeField] RoomGenerator_Manager roomGenerator;
    
    
    private void OnEnable()
    {
        eventSystem.CallRespawnToLastRespawner += RespawnPlayer;
    }
    private void OnDisable()
    {
        eventSystem.CallRespawnToLastRespawner -= RespawnPlayer;
    }
    public static Player_RespawnerManager Instance;
    private void Awake()
    {
        //ActivateSavedRespawner();

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
        if (!isSorted)
        {
            sortRespawners();
            isSorted = true;
        }
        return Respawners[GetFurthestActiveRespawnerIndex()];
    }
    int GetFurthestActiveRespawnerIndex()
    {
        int furthestIndex = 0;
        for (int i = 0; i < Respawners.Count; i++)
        {
            if (Respawners[i].IsActivated)
            {
                furthestIndex = i;
            }
        }
        //gameState.FurthestDoorsArray[roomGenerator.AreaIndex] = furthestIndex;
        return furthestIndex;
    }
    void sortRespawners()
    {
        setDistancesOfRespawners();

        //We should get back to just "Get furthest spawner()" without sorting pls

        int respawneresLenght = Respawners.Count;
        for (int i = 0; i < respawneresLenght; i++)
        {
            int closestIndex = i;
            for (int j = i +1; j < respawneresLenght; j++)
            {
                if (Respawners[j].distanceToManager < Respawners[closestIndex].distanceToManager)
                {
                    closestIndex = j;
                }
            }

            Player_Respawner tempRespawner = Respawners[closestIndex];
            Respawners[closestIndex] = Respawners[i];
            Respawners[i] = tempRespawner;
        }
    }
    void setDistancesOfRespawners()
    {
        foreach (Player_Respawner respawner in Respawners)
        {
            respawner.distanceToManager = (respawner.transform.position - transform.position).sqrMagnitude;
        }
    }
}
