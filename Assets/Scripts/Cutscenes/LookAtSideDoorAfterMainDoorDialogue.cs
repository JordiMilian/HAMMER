using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtSideDoorAfterMainDoorDialogue : BaseCutsceneLogic
{
    [SerializeField] Dialoguer mainDoorDialoguer;
    [SerializeField] Transform sideDoorTargetTf;

    private void OnEnable()
    {
        mainDoorDialoguer.onFinishedReading += checkIfProperDialogue;
    }
    void checkIfProperDialogue(int index)
    {
        if(index == 0)
        {
            CutscenesManager.Instance.AddCutscene(this);
        }
    }
    public override void playThisCutscene()
    {
        StartCoroutine(cutsceneLookAtDoor());
    }
    IEnumerator cutsceneLookAtDoor()
    {
        TargetGroupSingleton targetGroup = TargetGroupSingleton.Instance;
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

       

        yield return new WaitForSeconds(0.2f);
        playerStateMachine.ForceChangeState(playerRefs.DisabledState);
        targetGroup.SetOnlyOneTarget(sideDoorTargetTf, 1, 1);
        yield return new WaitForSeconds(2.5f);
        targetGroup.ReturnPlayersTarget();
        targetGroup.RemoveTarget(sideDoorTargetTf);

        yield return new WaitForSeconds(0.2f);
        playerStateMachine.ForceChangeState(playerRefs.IdleState);



    }
}
