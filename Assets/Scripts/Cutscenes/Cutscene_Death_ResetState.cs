using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_Death_ResetState : BaseCutsceneLogic
{
    [SerializeField] GameState gameState;
    public bool dontResetState;
    public override void playThisCutscene()
    {

        StartCoroutine(cutscene());
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
    IEnumerator cutscene()
    {
        if (!dontResetState) { gameState.ResetState(); }

        Player_References playerRefs = GlobalPlayerReferences.Instance.references;

        //playerRefs.events.CallHideAndDisable?.Invoke();
        playerRefs.disableController.HideAndDisablePlayer();

        SetupForRespwan();

        yield return null;

        RespawnersManager.Instance.RespawnPlayer();

        onCutsceneOver?.Invoke();


    }
}
