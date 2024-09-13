using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterExitScene_withDistance : EnterExitScene_controller
{
   [HideInInspector] public float DistanceToManager;
   public Player_Respawner tiedEnemyRespawner;
    public bool isDoorActive;
    public Action onDoorActivated;
    private void Awake()
    {
        FurthestDoor_Manager.Instance.enterExitScenesList.Add(this);
    }
    private void OnEnable()
    {
        tiedEnemyRespawner.OnRespawnerActivated += ActivateDoor;
    }
    private void OnDisable()
    {
        tiedEnemyRespawner.OnRespawnerActivated -= ActivateDoor;
    }
    private void Start()
    {
        if (playEnteringCutsceneOnLoad)
        {
            OnPlayerEnteredFromHere();
        }
    }
    void ActivateDoor()
    {
        isDoorActive = true;
        onDoorActivated?.Invoke();
    }

    
}
