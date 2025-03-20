using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enterSceneCutscene : BaseCutsceneLogic
{
    [SerializeField] Transform enteringPos;
    public override void playThisCutscene()
    {
        currentCutscene =  StartCoroutine(playCutscene()); 
    }
    IEnumerator playCutscene()
    {
        Debug.Log("moving player");
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;
        Transform playerTf = playerRefs.gameObject.transform;

        playerStateMachine.ForceChangeState(playerRefs.DisabledState);
        playerTf.position = enteringPos.position;

        //playerRefs.animator.SetTrigger("EnterRoom Lo que sigue etc"); 

        yield return new WaitForSeconds(.5f); // esperar a que acabe l'animacio

        playerStateMachine.ForceChangeState(playerRefs.IdleState);

        onCutsceneOver?.Invoke();
    }
}
