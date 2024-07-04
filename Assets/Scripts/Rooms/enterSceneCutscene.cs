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
        
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Transform playerTf = playerRefs.gameObject.transform;

        playerRefs.events.CallDisable?.Invoke();
        playerTf.position = enteringPos.position;

        //playerRefs.animator.SetTrigger("EnterRoom Lo que sigue etc"); 

        yield return new WaitForSeconds(.5f); // esperar a que acabe l'animacio

        playerRefs.events.CallEnable?.Invoke();

        onCutsceneOver?.Invoke();
    }
}
