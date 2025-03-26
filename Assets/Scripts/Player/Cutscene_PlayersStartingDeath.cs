using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene_PlayersStartingDeath : BaseCutsceneLogic
{
    [SerializeField] GameObject weaponPivot;
    [SerializeField] Player_References playerRefs;
    [SerializeField] GameState gameState;

    [SerializeField] BaseCutsceneLogic ResetStateCutscene;
    [SerializeField] BaseCutsceneLogic noResetCutscene;

    //This script also manages if we should restart the game or not after the proper cutscene
    //I feel like this is wrong probably but I dont want to split the responsability more

    private void OnEnable()
    {
        GameEvents.OnPlayerDeath += AddThisCutscene;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerDeath -= AddThisCutscene;
    }
    void AddThisCutscene()
    {
        CutscenesManager.Instance.AddCutscene(this);
    }
    public override void playThisCutscene()
    {
        Debug.Log("death ctscene");
        currentCutscene = StartCoroutine(cutsceneCoroutine());
    }
    IEnumerator cutsceneCoroutine()
    {
        gameState.playerDeaths++;
        //playerRefs.events.CallHideAndDisable?.Invoke(); WE ARE USING STATE MACHINE NOW

        SetupForRespwan();

        yield return new WaitForSeconds(3.5f); //Delay before teleport to tied enemy
        
        OnManageReset();

        onCutsceneOver?.Invoke();

    }
    void SetupForRespwan()
    {
        Transform weaponPivot = GlobalPlayerReferences.Instance.references.weaponPivot.transform;

        weaponPivot.transform.eulerAngles = new Vector3(
                    weaponPivot.transform.eulerAngles.x,
                    weaponPivot.transform.eulerAngles.y,
                    90
                    );
    }
    void OnManageReset()
    {
        CutscenesManager.Instance.AddCutscene(noResetCutscene);
        return;
    }
    bool checkIfReset()
    {
        return gameState.playerUpgrades.Count == 0 && gameState.isTutorialComplete;
    }

}
