using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Respawner : MonoBehaviour
{
    public event Action OnRespawnerActivated;
    public bool IsActivated = false;

    [SerializeField] Transform respawnPosition;
    [SerializeField] Enemy_EventSystem eventSystem;
    [SerializeField] Animator TiedEnemyAnimator;
    [SerializeField] TiedEnemy_StateController tiedStateController;
    
    
    GameObject RespawnedPlayer;
    RespawnersManager respawnerManager;
    [HideInInspector] public float distanceToManager;
    [HideInInspector] public int managerIndex;
    private void OnEnable()
    {
        respawnerManager =  RespawnersManager.Instance;
        respawnerManager.Respawners.Add(this);
    }
    private void OnDisable()
    {
        respawnerManager.Respawners.Remove(this);
        
    }
    void OnTiedEnemyKilled(DeadCharacterInfo info)
    {
        ActivateRespawner();
    }
    public void ExternallyActivateRespawner()
    {
        ActivateRespawner();
        //THere should be something to controll the visible sprites and shi. Mainly the head should disspear if activated
        GetComponent<IChangeStateByType>().ChangeStateByType(StateTags.Dead);
    }
    public void ActivateRespawner()
    {
        IsActivated = true;
        if (OnRespawnerActivated != null) OnRespawnerActivated();
    }
    public void RespawnFromHere(GameObject player)
    {
        player.transform.position = respawnPosition.position;
        RespawnedPlayer = player;
        StartCoroutine(delayedAnimation());
    }
    IEnumerator delayedAnimation()
    {
        yield return new WaitForSeconds(0.8f);
        TiedEnemyAnimator.SetTrigger("Reborn");
    }
    public void EV_ActivatePlayer()
    {
        RespawnedPlayer.GetComponent<Player_EventSystem>().CallShowAndEnable?.Invoke(); // Go to Player_StateMachine
    }
}
