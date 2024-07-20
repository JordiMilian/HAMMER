using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutscenesManager : MonoBehaviour
{
    public static CutscenesManager Instance;

    public Queue<BaseCutsceneLogic> cutscenesQueue = new Queue<BaseCutsceneLogic>();
    public bool isPlaying;

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
        Debug.Log("Added cutscene: " + cutscene.gameObject.name);
        if (!isPlaying)
        {
            playCutscene(cutscene);
        }
    }
    void playCutscene(BaseCutsceneLogic cutscene)
    {
        Debug.Log("Starting new cutscene: " + cutscene.gameObject.name);
        cutscene.playThisCutscene();
        isPlaying = true;
        cutscene.onCutsceneOver += CutsceneOver;
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
}
