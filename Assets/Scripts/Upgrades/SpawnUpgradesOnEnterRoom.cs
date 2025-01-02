using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUpgradesOnEnterRoom : MonoBehaviour
{
    [SerializeField] Generic_OnTriggerEnterEvents onEnterRoomCollider;
    [SerializeField] Cutscene_SpawnUpgradesGroup spawnUpgradesGroup;

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
        CutscenesManager.Instance.AddCutscene(spawnUpgradesGroup);
        onEnterRoomCollider.RemoveActivatorTag(Tags.Player_SinglePointCollider);
    }
}
