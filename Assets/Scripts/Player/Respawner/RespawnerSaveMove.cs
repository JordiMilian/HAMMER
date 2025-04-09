using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnerSaveMove : MonoBehaviour
{
    [SerializeField] RespawnersManager respawnerManager;
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
    void checkIfNullAndRespawn(TiedEnemy_Controller respawner)
    {
        if(respawner != null) 
        {
            GameObject playerGO = GlobalPlayerReferences.Instance.references.gameObject;
            respawner.MovePlayerHere(playerGO); 
        }
    }
}
