using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DeathCutscene : BaseCutsceneLogic
{
    [SerializeField] GameObject weaponPivot;
    [SerializeField] Player_References playerRefs;
    private void OnEnable()
    {
        GameEvents.OnPlayerDeath += playThisCutscene;
    }
    public override void playThisCutscene()
    {

        StartCoroutine(cutsceneCoroutine());
    }
    IEnumerator cutsceneCoroutine()
    {
        playerRefs.events.CallHideAndDisable();

        SetupForRespwan();

        yield return new WaitForSeconds(3.5f); //Delay before teleport to tied enemy

        playerRefs.events.CallRespawn?.Invoke(); 

        playerRefs.healthSystem.RestoreAllHealth();

    }
    void SetupForRespwan()
    {
        weaponPivot.transform.eulerAngles = new Vector3(
                    weaponPivot.transform.eulerAngles.x,
                    weaponPivot.transform.eulerAngles.y,
                    90
                    );
    }

}
