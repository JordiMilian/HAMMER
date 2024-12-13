using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCutsceneLogic : MonoBehaviour
{
    public Coroutine currentCutscene;
    public abstract void playThisCutscene();
    public Action onCutsceneOver;

    //All cutscenes MUST implement the onCutsceneOver Action at the end of themselfs or the cutscene will never end
    public void stopCurrentCutscene()
    {
        if(currentCutscene != null)
        {
            StopCoroutine(currentCutscene);
        }
        //Transform testTf = transform;
        //try { testTf.position = Vector2.zero; }
        //catch(Exception e) { Debug.LogError(e); }
    }

}
