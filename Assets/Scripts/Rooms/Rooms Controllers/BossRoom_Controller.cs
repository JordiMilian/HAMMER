using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom_Controller : MonoBehaviour, IRoom, IRoomWithEnemies, IMultipleRoom, ICutsceneable
{
    public Action OnAllEnemiesKilled { get; set; }
    public Action OnEnemiesSpawned {  get; set; }
    public List<GameObject> CurrentlySpawnedEnemies { get; set; }
    [SerializeField] GameObject BossPrefab;
    [SerializeField] Transform Tf_bossSpawnPoint;
    [SerializeField] UI_BossHealthBar healthBar;
    [SerializeField] Transform Tf_EndingCutseneableHolder;
    [SerializeField] UpgradesGroup upgradesGroup;
    [SerializeField] DoorAnimationController ExitDoor;
    [SerializeField] GameState gameState;
    [SerializeField] int IndexInGameState;
    #region MULTIPLE ROOMS INFO

    [SerializeField] Transform _tf_ExitPos;
    public Vector2 ExitPos => _tf_ExitPos.position;

    [SerializeField] Generic_OnTriggerEnterEvents _combinedCollider;
    public Generic_OnTriggerEnterEvents combinedCollider => _combinedCollider;
    #endregion
    public void OnRoomLoaded()
    {
        CurrentlySpawnedEnemies = new();
        GameObject instantiatedBoss = Instantiate(BossPrefab, Tf_bossSpawnPoint.position, Quaternion.identity, transform);

        CurrentlySpawnedEnemies.Add(instantiatedBoss);
        OnEnemiesSpawned?.Invoke();

        CutscenesManager.Instance.AddCutsceneable(this);
        instantiatedBoss.GetComponent<IKilleable>().OnKilled_event += onBossKilled;
        ExitDoor.DisableAutoDoorOpener();
        GameEvents.OnPlayerDeath += CutMusic;
        //Subscribe to Boss death to finish room
    }
    public void OnRoomUnloaded()
    {
        CutMusic();
        GameEvents.OnPlayerDeath -= CutMusic;
    }
    void onBossKilled(DeadCharacterInfo info)
    {
        upgradesGroup.transform.position = info.DeadGameObject.transform.position;
        CutscenesManager.Instance.AddCutsceneable(upgradesGroup);

        if (Tf_EndingCutseneableHolder.TryGetComponent(out ICutsceneable endCutsce))
        {
            CutscenesManager.Instance.AddCutsceneable(endCutsce);
        }
        else { Debug.LogError("Missing boss defeated cutscene"); }

        
       

        CurrentlySpawnedEnemies = new();
        CutMusic();

        gameState.FourDoors[IndexInGameState].isCompleted = true;
        OnAllEnemiesKilled?.Invoke();
    }
    #region ENTER ROOM CUTSCENE
    public IEnumerator ThisCutscene()
    {
        //Find the references
        CameraZoomController zoomer = GameObject.Find(Tags.CMvcam1).GetComponent<CameraZoomController>();
        Transform bossTf = CurrentlySpawnedEnemies[0].transform;
        TargetGroupSingleton targetGroup = TargetGroupSingleton.Instance;
        Enemy_References bossRefs = bossTf.GetComponent<Enemy_References>();
        Generic_StateMachine bossStateMachine = bossTf.GetComponent<Generic_StateMachine>();
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;

        yield return null; //wait one frame so the game controller can set player into entering
        //disable player

        Player_StateMachine playerStateMachine = playerRefs.stateMachine;
        playerStateMachine.ForceChangeState(playerRefs.DisabledState);
        yield return new WaitForSeconds(0.4f);


        //Zoom and target the Boss
        float zoomToBoss = 4;
        zoomer.AddZoomInfoAndUpdate(new CameraZoomController.ZoomInfo(zoomToBoss, 3, "enterCutscene"));
        targetGroup.SetOnlyOneTarget(bossTf, 50, 1);
        PlayMusic();

        //Boss intro and wait for it to end

        EnemyState BossIntroState = bossTf.GetComponent<Boss_Controller>().BossIntroAnimationState;
        bossStateMachine.ChangeState(BossIntroState); //this state auto transition to Idle when over
        yield return null; //Wait one frame in case of transition or whatever
        while (bossStateMachine.currentState == BossIntroState)
        {
            yield return null;
        }
        healthBar.ShowCanvas();
        

        yield return new WaitForSeconds(.3f);

        //return to basic zooms
        zoomer.RemoveZoomInfoAndUpdate("enterCutscene");
        targetGroup.ReturnPlayersTarget();
        targetGroup.RemoveTarget(bossTf);
        playerRefs.swordRotation.FocusNewEnemy(bossRefs.Focuseable);


        //enable player again
        playerStateMachine.ForceChangeState(playerRefs.IdleState);

    }
    public void ForceEndCutscene()
    {
        //Find the references
        CameraZoomController zoomer = GameObject.Find(Tags.CMvcam1).GetComponent<CameraZoomController>();
        Transform bossTf = CurrentlySpawnedEnemies[0].transform;
        TargetGroupSingleton targetGroup = TargetGroupSingleton.Instance;
        Enemy_References bossRefs = bossTf.GetComponent<Enemy_References>();
        Generic_StateMachine bossStateMachine = bossTf.GetComponent<Generic_StateMachine>();
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;

        healthBar.ShowCanvas();

        bossStateMachine.ChangeState(bossRefs.AgrooState);

        zoomer.RemoveZoomInfoAndUpdate("enterCutscene");
        targetGroup.SetOnlyPlayerAndMouseTarget();
        playerRefs.swordRotation.FocusNewEnemy(bossRefs.Focuseable);

        //enable player again
        playerRefs.stateMachine.ForceChangeState(playerRefs.IdleState);
    }
    AudioSource musicSource;
    [SerializeField] AudioClip BattleMusic;
    private void PlayMusic()
    {
        musicSource = SFX_PlayerSingleton.Instance.FadeInMusic(BattleMusic);
    }
    void CutMusic()
    {
       SFX_PlayerSingleton.Instance.FadeOutMusic(musicSource);
    }

    #endregion


}
