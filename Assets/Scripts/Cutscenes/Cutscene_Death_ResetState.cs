using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_Death_ResetState : BaseCutsceneLogic
{
    [SerializeField] GameState gameState;
    public override void playThisCutscene()
    {
        gameState.ResetState();

        Player_References playerRefs = GlobalPlayerReferences.Instance.references;

        playerRefs.events.CallHideAndDisable?.Invoke();

        SetupForRespwan();

        playerRefs.events.CallRespawnToLastRespawner?.Invoke();

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
}
