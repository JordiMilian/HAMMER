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
        eventSystem.OnDeath += OnTiedEnemyKilled;
    }
    private void OnDisable()
    {
        respawnerManager.Respawners.Remove(this);
        eventSystem.OnDeath -= OnTiedEnemyKilled;
    }
    void OnTiedEnemyKilled(object sender, Enemy_EventSystem.DeadCharacterInfo info)
    {
        ActivateRespawner();
    }
    public void ExternallyActivateRespawner()
    {
        ActivateRespawner();
        //THere should be something to controll the visible sprites and shi. Mainly the head should disspear if activated
        GetComponent<TiedEnemy_StateMachine>().OnDeathState(this, new Generic_EventSystem.DeadCharacterInfo(gameObject,gameObject));
        tiedStateController.OnDeath(this, new Generic_EventSystem.DeadCharacterInfo(gameObject,gameObject));
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
