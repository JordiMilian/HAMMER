using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStarter_OutOfRunWorld : SceneStarter_base
{
    [Header("OutOfRun")]
    [SerializeField] GameState gameState;
    [SerializeField] PlayerStats currentPlayerStats;
    [SerializeField] PlayerStats basePlayerStats;
    [SerializeField] GameObject LevelPrefab;
    [SerializeField] AnimationClip spawnOutOfRunAnimation;
    public override IEnumerator Creation()
    {
        yield return base.Creation();
        LevelPrefab = Instantiate(LevelPrefab);
    }
    public override IEnumerator Preparation()
    {
        yield return StartCoroutine( base.Preparation());
        TransformLevelsToCurrency();
        gameState.FinishedRun();
    }
    void TransformLevelsToCurrency()
    {
        gameState.PermanentCurrency += currentPlayerStats.Level;
        currentPlayerStats.CopyData(basePlayerStats);
        
    }
    public override IEnumerator StartPlaying()
    {
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;

        playerRefs.flasher.CallCustomFlash(spawnOutOfRunAnimation.length, Color.white);
        playerRefs.animator.SetTrigger("SpawnOutOfRun");
        yield return new WaitForSeconds(spawnOutOfRunAnimation.length);
        playerRefs.stateMachine.ForceChangeState(playerRefs.IdleState);
    }
}
