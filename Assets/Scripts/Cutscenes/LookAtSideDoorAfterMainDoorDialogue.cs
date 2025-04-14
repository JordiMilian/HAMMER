using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtSideDoorAfterMainDoorDialogue : MonoBehaviour, ICutsceneable
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
            CutscenesManager.Instance.AddCutsceneable(this);
        }
    }

    public IEnumerator ThisCutscene()
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

    public void ForceEndCutscene()
    {
        TargetGroupSingleton targetGroup = TargetGroupSingleton.Instance;
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

        targetGroup.ReturnPlayersTarget();
        targetGroup.RemoveTarget(sideDoorTargetTf);
        playerStateMachine.ForceChangeState(playerRefs.IdleState);
    }
}
