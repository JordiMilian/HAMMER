using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene_Death_NoResetState : BaseCutsceneLogic
{
    [SerializeField] GameState gameState;
    public override void playThisCutscene()
    {
        currentCutscene = StartCoroutine(cutscene());
    }
    IEnumerator cutscene()
    {
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;

        playerRefs.upgradesManager.deleteRandomUpgrade();

        playerRefs.events.CallRespawnToLastRespawner?.Invoke();
        playerRefs.healthSystem.RestoreAllHealth();

        yield return new WaitForSeconds(.25f);
        onCutsceneOver?.Invoke();
    }
}
