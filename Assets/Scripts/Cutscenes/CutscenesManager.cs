using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenesManager : MonoBehaviour
{
    public static CutscenesManager Instance;

    Queue<BaseCutsceneLogic> cutscenesQueue = new Queue<BaseCutsceneLogic>();
    bool isPlaying;

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
    public void AddCutscene(BaseCutsceneLogic cutscene)
    {
        cutscenesQueue.Enqueue(cutscene);

        if (!isPlaying)
        {
            playCutscene(cutscene);
        }
    }
    void playCutscene(BaseCutsceneLogic cutscene)
    {
        cutscene.playThisCutscene();
        isPlaying = true;
        cutscene.onCutsceneOver += CutsceneOver;
    }
     void CutsceneOver()
    {
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
}
