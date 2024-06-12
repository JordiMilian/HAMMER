using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCutsceneLogic : MonoBehaviour
{
    public Coroutine currentCutscene;
    public abstract void playThisCutscene();

    public void stopCurrentCutscene()
    {
        if(currentCutscene != null)
        {
            StopCoroutine(currentCutscene);
        }
    }

}
