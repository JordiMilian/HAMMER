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
    [SerializeField] Audio_Area audioArea;
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
        OnEnemiesSpawned?.Invoke();

        CurrentlySpawnedEnemies.Add(instantiatedBoss);

        CutscenesManager.Instance.AddCutsceneable(this);
        instantiatedBoss.GetComponent<IKilleable>().OnKilled_event += onBossKilled;
        //Subscribe to Boss death to finish room
    }
    public void OnRoomUnloaded()
    {
        
    }
    void onBossKilled(DeadCharacterInfo info)
    {
        if(Tf_EndingCutseneableHolder.TryGetComponent(out ICutsceneable endCutsce))
        {
            CutscenesManager.Instance.AddCutsceneable(endCutsce);
        }
        else { Debug.LogError("Missing boss defeated cutscene"); }


        audioArea.FadeOutAudio(new Collider2D());
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


        //disable player

        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

        playerStateMachine.ForceChangeState(playerRefs.DisabledState);

        //Zoom and target the Boss
        float zoomToBoss = 2;
        zoomer.AddZoomInfoAndUpdate(new CameraZoomController.ZoomInfo(zoomToBoss, 3, "enterCutscene"));
        targetGroup.SetOnlyOneTarget(bossTf, 50, 1);

        //Boss intro and wait for it to end
        
        EnemyState BossIntroState = bossTf.GetComponent<Boss_Controller>().BossIntroAnimationState;
        bossStateMachine.ChangeState(BossIntroState); //this state auto transition to Idle when over
        yield return null; //Wait one frame in case of transition or whatever
        while (bossStateMachine.currentState == BossIntroState)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        healthBar.ShowCanvas();


        bossStateMachine.ChangeState(bossRefs.AgrooState);

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

    #endregion

    
}
