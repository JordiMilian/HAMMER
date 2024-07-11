using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Respawner : MonoBehaviour
{
    [SerializeField] Transform respawnPosition;
    [SerializeField] Enemy_EventSystem eventSystem;
    [SerializeField] Animator TiedEnemyAnimator;
    public event Action OnRespawnerActivated;
    public bool IsActivated = false;
    GameObject RespawnedPlayer;
    Player_RespawnerManager respawnerManager;
    [HideInInspector] public float distanceToManager;
    [HideInInspector] public int managerIndex;
    private void OnEnable()
    {
        respawnerManager =  Player_RespawnerManager.Instance;
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
    public void ExternallyActivateRespawner() //This is called from the AltoMando when loading the scene after a reset
    {
        ActivateRespawner();
        //Logic to hide sprites of the Head of the tied enemy PLSSS
    }
     void ActivateRespawner()
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
