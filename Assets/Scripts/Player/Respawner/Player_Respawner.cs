using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Respawner : MonoBehaviour
{
    [SerializeField] Transform respawnPosition;
    [SerializeField] Enemy_EventSystem eventSystem;
    public event Action OnRespawnerActivated;
    public bool IsActivated = false;
    private void OnEnable()
    {
        eventSystem.OnDeath += OnDudeKilled;
    }
    private void OnDisable()
    {
        eventSystem.OnDeath -= OnDudeKilled;
    }
    void OnDudeKilled(object sender, Enemy_EventSystem.DeadCharacterInfo info)
    {
        ActivateRespawner();
    }
     void ActivateRespawner()
    {
        IsActivated = true;
        if (OnRespawnerActivated != null) OnRespawnerActivated();
    }
    public void RespawnFromHere(GameObject player)
    {
        player.transform.position = respawnPosition.position;
    }
}
