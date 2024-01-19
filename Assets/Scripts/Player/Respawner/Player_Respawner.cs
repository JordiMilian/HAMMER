using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Respawner : MonoBehaviour
{

    [SerializeField] Transform respawnPosition;
    public event EventHandler OnRespawnerActivated;
    public bool IsActivated = false;
    public void ActivateRespawner()
    {
        IsActivated = true;
        if (OnRespawnerActivated != null) OnRespawnerActivated(this, EventArgs.Empty);
    }
    public void RespawnFromHere(GameObject player)
    {
        player.transform.position = respawnPosition.position;
    }
}
