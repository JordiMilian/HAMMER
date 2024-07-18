using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbsManger : MonoBehaviour
{
    //KILL THIS SCRIPT NOW

    [SerializeField] List<OrbLogic> orbLogicList;
    [SerializeField] Player_Respawner respawner;
    bool IsCollected;
    private void OnEnable()
    {
        respawner.OnRespawnerActivated += SpawnOrbs;
    }
    private void OnDisable()
    {
        respawner.OnRespawnerActivated -= SpawnOrbs;
    }

    void SpawnOrbs()
    {
        if(IsCollected) { return; }

        foreach(OrbLogic orb in orbLogicList)
        {
            if(orb != null)
            {
                orb.Spawn();
                orb.OnPickedUp += DespawnOrbs;
            }
        }
        IsCollected = true;
    }
    void DespawnOrbs()
    {
        foreach (OrbLogic orb in orbLogicList)
        {
            if(orb != null)
            {
                if (orb.isCollected) { continue; }
                orb.Despawn();
            }
        }
    }
}
