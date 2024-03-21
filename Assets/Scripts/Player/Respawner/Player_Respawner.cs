using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Respawner : MonoBehaviour
{

    [SerializeField] Transform respawnPosition;
    [SerializeField] Generic_OnTriggerEnterEvents ActivationTrigger;
    public event Action OnRespawnerActivated;
    public bool IsActivated = false;
    private void OnEnable()
    {
        ActivationTrigger.AddActivatorTag(TagsCollection.Attack_Hitbox);
        ActivationTrigger.OnTriggerEntered += OnDudeKilled;
    }
    private void OnDisable()
    {
        ActivationTrigger.OnTriggerEntered -= OnDudeKilled;
    }
    void OnDudeKilled(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo info)
    {
        ActivateRespawner();
    }
    public void ActivateRespawner()
    {
        IsActivated = true;
        if (OnRespawnerActivated != null) OnRespawnerActivated();
    }
    public void RespawnFromHere(GameObject player)
    {
        player.transform.position = respawnPosition.position;
    }
}
