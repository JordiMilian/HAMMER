using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUpgradesOnEnterRoom : MonoBehaviour
{
    [SerializeField] Generic_OnTriggerEnterEvents onEnterRoomCollider;
    [SerializeField] UpgradesGroup spawnUpgradesGroup;

    private void OnEnable()
    {
        onEnterRoomCollider.AddActivatorTag(Tags.Player_SinglePointCollider);
        onEnterRoomCollider.OnTriggerEntered += spawnUpgrades;
    }
    private void OnDisable()
    {
        onEnterRoomCollider.OnTriggerEntered -= spawnUpgrades;
    }
    void spawnUpgrades(Collider2D colision)
    {
        CutscenesManager.Instance.AddCutsceneable(spawnUpgradesGroup);
        onEnterRoomCollider.RemoveActivatorTag(Tags.Player_SinglePointCollider);
    }
}
