using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_SpawnUpgradesGroup : BaseCutsceneLogic
{
    [SerializeField] UpgradesGroup upgradesGroup;
    TargetGroupSingleton targetGroupSingleton;
    public override void playThisCutscene()
    {
        currentCutscene = StartCoroutine(spawnCutscene());   
    }
    IEnumerator spawnCutscene()
    {
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

        playerStateMachine.ForceChangeState(playerRefs.DisabledState);

        targetGroupSingleton = TargetGroupSingleton.Instance;
        
        targetGroupSingleton.AddTarget(upgradesGroup.transform, 10, 1);
        targetGroupSingleton.RemovePlayersTarget();

        yield return new WaitForSeconds(1);

        upgradesGroup.onSpawnNewContainers();

        yield return new WaitForSeconds(1);

        targetGroupSingleton.RemoveTarget(upgradesGroup.transform);
        targetGroupSingleton.ReturnPlayersTarget();

        playerStateMachine.ForceChangeState(playerRefs.IdleState);

        onCutsceneOver?.Invoke();
    }
}
