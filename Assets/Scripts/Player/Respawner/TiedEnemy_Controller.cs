using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialoguesContainer;

public class TiedEnemy_Controller : MonoBehaviour, IDamageReceiver
{
    public event Action OnRespawnerActivated;
    public bool IsActivated = false;
    public Transform RespawnPosition;
    [SerializeField] Animator TiedEnemyAnimator;
    
    GameObject RespawnedPlayer;

    //Stuff for the Manager
    RespawnersManager respawnerManager;
    [HideInInspector] public float distanceToManager;
    private void OnEnable()
    {
        subToDialoguer();

        respawnerManager = RespawnersManager.Instance;
        respawnerManager.Respawners.Add(this);
    }
    private void OnDisable()
    {
        respawnerManager.Respawners.Remove(this);
    }
    public void ActivateRespawner(bool withFeedback = true)
    {
        if (IsActivated) { Debug.Log("Activated already active respawner"); return; }

        IsActivated = true;
        if (withFeedback) { KilledFeedback(); }

        dialoguer.enabled = false;
        OnRespawnerActivated?.Invoke();
    }
    public void MovePlayerHere(GameObject player)
    {
        player.transform.position = RespawnPosition.position;
        RespawnedPlayer = player;
    }
    #region KILLED FEEDBACK
    [Header("Feedbacks")]
    [SerializeField] GameObject HeadSpriteTf;
    [SerializeField] SpriteRenderer[] bodySprites;
    [SerializeField] DeadPart_Instantiator deadPartInstantiator;
    [SerializeField] AudioClip SFX_KilledSound;
    [SerializeField] Generic_Flash flasher;
    void KilledFeedback()
    {
        HideHead();
        GameObject[] deadParts = deadPartInstantiator.InstantiateDeadParts();
        SFX_PlayerSingleton.Instance.playSFX(SFX_KilledSound);
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.BloodExplosion, deadParts[0].transform.position);
        TimeScaleEditor.Instance.SlowMotion(IntensitiesEnum.Medium);
    }

    #endregion
    #region SHOW/HIDE BODY PARTS
    void HideHead() { HeadSpriteTf.GetComponent<SpriteRenderer>().enabled = false; }
    void ShowHead() { HeadSpriteTf.GetComponent<SpriteRenderer>().enabled = true; }
    void HideBody()
    {
        foreach (SpriteRenderer sprite in bodySprites) { sprite.enabled = false; }
    }
    void ShowBody()
    {
        foreach (SpriteRenderer sprite in bodySprites) { sprite.enabled = true; }
    }
    #endregion
    #region DAMAGE RECEIVED
    public Action<ReceivedAttackInfo> OnDamageReceived_event { get; set; }
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        //play animation from animator directament o que? vfx si eso i ja, coses generiques

        flasher.CallDefaultFlasher();
        TiedEnemyAnimator.SetTrigger("Hit");
        OnDamageReceived_event?.Invoke(info); //The dialoguer listens to this Event 
    }
    #endregion
    #region DIALOGUER
    [SerializeField] Dialoguer_reference dialoguerReference;
    Dialoguer dialoguer;

    void subToDialoguer()
    {
        dialoguer = dialoguerReference.Dialoguer;
        dialoguer.onFinishedReading += OnFinishedReading;
    }
    void OnFinishedReading(int nose)
    {
        ActivateRespawner(true);
        dialoguer.onFinishedReading -= OnFinishedReading;
    }
    #endregion
    #region CALLED FROM PLAYER_RESPAWNSTATE
    public void PlayRespawningAnimation()
    {
        TiedEnemyAnimator.SetTrigger("Reborn");
        ShowBody();
    }
    public void EV_ActivatePlayer()
    {
        PlayerState_Respawning playerRespawnState = (PlayerState_Respawning)RespawnedPlayer.GetComponent<Player_References>().RespawningState;
        playerRespawnState.EV_ActuallyRespawn();
        HideBody();
        SFX_PlayerSingleton.Instance.playSFX(SFX_KilledSound);
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.BloodExplosion, playerRespawnState.transform.position);

    }
    #endregion
}
