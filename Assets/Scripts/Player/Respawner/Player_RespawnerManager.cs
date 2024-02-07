using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_RespawnerManager : MonoBehaviour
{

    [SerializeField] List<Player_Respawner> Respawners = new List<Player_Respawner>();
    [SerializeField] Player_Respawner CurrentFurthestRespawner;
    [SerializeField] GameObject PlayerGO;
    Generic_HealthSystem playerHealth;

    private void OnEnable()
    {
        playerHealth = PlayerGO.GetComponent<Generic_HealthSystem>();
        playerHealth.OnDeath += RespawnPlayer;
        foreach (Player_Respawner respawner in Respawners)
        {
            respawner.OnRespawnerActivated += CheckFurthestRespawner;
        }
    }
    private void OnDisable()
    {
        playerHealth.OnDeath -= RespawnPlayer;
        foreach (Player_Respawner respawner in Respawners)
        {
            respawner.OnRespawnerActivated -= CheckFurthestRespawner;
        }
    }
    void RespawnPlayer(object sender, EventArgs args)
    {
        CheckFurthestRespawner(this, EventArgs.Empty);
        CurrentFurthestRespawner.RespawnFromHere(PlayerGO);
        playerHealth.RestoreAllHealth();
    }
    void CheckFurthestRespawner(object sender, EventArgs args)
    {
        CurrentFurthestRespawner = FindFurthestActiveRespawner();
    }
    Player_Respawner FindFurthestActiveRespawner()
    {
        Player_Respawner Furthest = new Player_Respawner();
        foreach(Player_Respawner respawner in Respawners)
        {
            if (respawner.IsActivated) { Furthest = respawner; }
        }
        return Furthest;
    }
}