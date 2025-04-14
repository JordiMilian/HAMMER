using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CutscenesManager : MonoBehaviour
{
    public static CutscenesManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    
    public Queue<BaseCutsceneLogic> cutscenesQueue = new Queue<BaseCutsceneLogic>();
    public bool isPlaying;

   
    public void AddCutscene(BaseCutsceneLogic cutscene)
    {
        cutscenesQueue.Enqueue(cutscene);
        Debug.Log("Added cutscene: " + cutscene.gameObject.name);
        if (!isPlaying)
        {
            playCutscene(cutscene);
        }
    }
    void playCutscene(BaseCutsceneLogic cutscene)
    {
        Debug.Log("Starting new cutscene: " + cutscene.gameObject.name);
        cutscene.onCutsceneOver += CutsceneOver;
        cutscene.playThisCutscene();
        isPlaying = true;
    }
     void CutsceneOver()
    {
        Debug.Log("Cutscene finished");
        cutscenesQueue.Peek().onCutsceneOver -= CutsceneOver;

        cutscenesQueue.Dequeue();

        if(cutscenesQueue.Count > 0)
        {
            playCutscene(cutscenesQueue.Peek());
        }
        else
        {
            isPlaying = false;
        }
    }
    
    Coroutine currentPlayingCutscene;
    Queue<ICutsceneable> CutsceneablesQueue = new Queue<ICutsceneable>();
    public void AddCutsceneable(ICutsceneable cutsceneable)
    {
        CutsceneablesQueue.Enqueue(cutsceneable);
        if (CutsceneablesQueue.Count == 1)
        {
            currentPlayingCutscene = StartCoroutine(playCutsceneable(cutsceneable));
        }
    }
    public void ForceNextCutscene(ICutsceneable cutsceneable)
    {
        //Stop the current cutscene
        if(currentPlayingCutscene != null)
        {
            StopCoroutine(currentPlayingCutscene);
        }
        //Force end the current and the rest of cutscenes
        while (CutsceneablesQueue.Count > 0)
        {
            CutsceneablesQueue.Peek().ForceEndCutscene();
            CutsceneablesQueue.Dequeue();
        }
        AddCutsceneable(cutsceneable);
    }

    //Go throw all the steps of the coroutine. If it catches an error, break the cutscene and try to force it (every cutscene needs a method to call in case of forcing)
    //Either way, if the cutscene is over, look at the queue and start the next cutscene if there is any
    public IEnumerator playCutsceneable(ICutsceneable cutscene)
    {
        IEnumerator thisCutscene = cutscene.ThisCutscene();
        object currentYield;
        while (true)
        {
            try
            {
                if(!thisCutscene.MoveNext())
                {
                    break;
                }
                currentYield = thisCutscene.Current;
            }
            catch(System.Exception e)
            {
                Debug.LogError(cutscene+ " got interrupted: " + e);
                try
                {
                    cutscene.ForceEndCutscene();
                }
                catch(System.Exception e2)
                {
                    Debug.LogError(cutscene + " could not be forceEnded: " + e2);
                }
                break;
            }
            yield return currentYield;
        }
        //Cutscene is over, handle next cutsceneable
        CutsceneablesQueue.Dequeue();
        if(CutsceneablesQueue.Count > 0)
        {
            currentPlayingCutscene = StartCoroutine(playCutsceneable(CutsceneablesQueue.Peek()));
        }
    }
}
