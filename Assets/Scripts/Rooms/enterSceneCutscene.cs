using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enterSceneCutscene : BaseCutsceneLogic
{
    [SerializeField] Transform enteringPos;
    public override void playThisCutscene()
    {
        StartCoroutine(playCutscene()); 
    }
    IEnumerator playCutscene()
    {
        Transform playerTf = GameObject.Find(TagsCollection.MainCharacter).transform;
        Player_References playerRefs = playerTf.GetComponent<Player_References>();

        playerRefs.events.CallDisable?.Invoke();
        playerTf.position = enteringPos.position;

        //playerRefs.animator.SetTrigger("EnterRoom Lo que sigue etc"); 

        yield return new WaitForSeconds(.1f); // esperar a que acabe l'animacio

        playerRefs.events.CallEnable?.Invoke();
    }
}
