using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossDefeated_Cutsceneable : MonoBehaviour, ICutsceneable 
{
    [SerializeField] Animator BossDefeatedUI_animator;
    [SerializeField] AnimationClip BossDefeatedUiClip;
    [SerializeField] AnimationClip openDoorAnimation;
    [SerializeField] DoorAnimationController doorController;
    [SerializeField] GameState gameState;
    [SerializeField] float delayBeforeUI;
    [SerializeField] float delayBeforeDoor;
    [SerializeField] UpgradesGroup upgradeGroup;
    [SerializeField] bool skipUpgrades;



    public IEnumerator ThisCutscene()
    {
        Transform doorTransform = doorController.transform;
        TargetGroupSingleton targetGroup = TargetGroupSingleton.Instance;
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

        yield return new WaitForSeconds(delayBeforeUI);

        playerStateMachine.ForceChangeState(playerRefs.DisabledState);
        BossDefeatedUI_animator.SetTrigger("BossDefeated");
        yield return new WaitForSeconds(BossDefeatedUiClip.length);


        yield return new WaitForSeconds(delayBeforeDoor); //Wait before door

        //Open door
        targetGroup.SetOnlyOneTarget(doorTransform, 10, 5); 
        yield return new WaitForSeconds(0.2f);
        doorController.OpenDoor(); 
        yield return new WaitForSeconds(openDoorAnimation.length + 0.3f); 
        targetGroup.SetOnlyPlayerAndMouseTarget();


        playerStateMachine.ForceChangeState(playerRefs.IdleState);

        if (!skipUpgrades) { upgradeGroup.StartSpawnCutscene(); }

    }

    public void ForceEndCutscene()
    {
        TargetGroupSingleton targetGroup = TargetGroupSingleton.Instance;
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

        doorController.OpenDoor();
        targetGroup.SetOnlyPlayerAndMouseTarget();
        playerStateMachine.ForceChangeState(playerRefs.IdleState);

        upgradeGroup.StartSpawnCutscene();
    }
}
