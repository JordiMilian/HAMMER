using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnerSaveMove : MonoBehaviour
{
    [SerializeField] Player_RespawnerManager respawnerManager;
    [SerializeField] GameObject playerGO;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            checkIfNullAndRespawn(respawnerManager.Respawners[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { checkIfNullAndRespawn(respawnerManager.Respawners[1]); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { checkIfNullAndRespawn(respawnerManager.Respawners[2]); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { checkIfNullAndRespawn(respawnerManager.Respawners[3]); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { checkIfNullAndRespawn(respawnerManager.Respawners[4]); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { checkIfNullAndRespawn(respawnerManager.Respawners[5]); }
    }
    void checkIfNullAndRespawn(Player_Respawner respawner)
    {
        if(respawner != null) { respawner.RespawnFromHere(playerGO); }
    }
}
