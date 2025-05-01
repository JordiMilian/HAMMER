using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom_Controller : MonoBehaviour, IRoom, IRoomWithEnemies, IMultipleRoom, ICutsceneable
{
    public Action OnAllEnemiesKilled { get; set; }
    public List<GameObject> CurrentlySpawnedEnemies { get; set; }
    #region MULTIPLE ROOMS INFO

    [SerializeField] Transform _tf_ExitPos;
    public Vector2 ExitPos => _tf_ExitPos.position;

    [SerializeField] Generic_OnTriggerEnterEvents _combinedCollider;
    public Generic_OnTriggerEnterEvents combinedCollider => _combinedCollider;
    #endregion
    public void OnRoomLoaded()
    {
        //spawn boss
        //add it to currentlySpawnedEnemies
        //boss healthbar
        //play boss intro animation 
        //Subscribe to Boss death to finish room
    }
    public void OnRoomUnloaded()
    {
        
    }
    void onBossKilled()
    {
        //Boss cutscene
        OnAllEnemiesKilled?.Invoke();
    }
    #region CUTSCENE
    public IEnumerator ThisCutscene()
    {
        //BossRoomCutscene
        yield return null;
    }
    public void ForceEndCutscene()
    {

    }

    #endregion

    
}
