using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene_Death_NoResetState : BaseCutsceneLogic
{
    public override void playThisCutscene() { }
    /* I HATE THE CUTSCENES
     * 
     * 
     * 
    [SerializeField] GameState gameState;
    [SerializeField] Animator iconAnimator;
    [SerializeField] SpriteRenderer iconRenderer;
    public override void playThisCutscene()
    {
        currentCutscene = StartCoroutine(cutsceneWithoutIcon());
        return;
        if (gameState.playerUpgrades.Count > 0)
        {
            currentCutscene = StartCoroutine(cutsceneWithIcon());
        }
        else
        {
            currentCutscene = StartCoroutine(cutsceneWithoutIcon());
        }
        
    }
    IEnumerator cutsceneWithIcon()
    {
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;

        yield return new WaitForSeconds(0.1f);

        playerRefs.upgradesManager.deleteRandomUpgrade();

        iconRenderer.sprite = gameState.lastLostUpgrade.iconSprite;

        iconAnimator.SetTrigger("Lost");

        yield return new WaitForSeconds(3.5f);


        RespawnersManager.Instance.RespawnPlayer();
        playerRefs.healthSystem.RestoreAllHealth();

        yield return new WaitForSeconds(.25f);
        onCutsceneOver?.Invoke();
    }
    IEnumerator cutsceneWithoutIcon()
    {
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        yield return new WaitForSeconds(0.1f);

        playerRefs.events.CallDisable();
        RespawnersManager.Instance.RespawnPlayer();
        playerRefs.healthSystem.RestoreAllHealth();

        yield return new WaitForSeconds(.25f);
        onCutsceneOver?.Invoke();
    }
    */
}
