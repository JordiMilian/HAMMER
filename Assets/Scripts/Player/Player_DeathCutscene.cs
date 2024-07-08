using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DeathCutscene : BaseCutsceneLogic
{
    [SerializeField] GameObject weaponPivot;
    [SerializeField] Player_References playerRefs;
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
        currentCutscene = StartCoroutine(cutsceneCoroutine());
    }
    IEnumerator cutsceneCoroutine()
    {
        playerRefs.events.CallHideAndDisable();

        SetupForRespwan();

        yield return new WaitForSeconds(3.5f); //Delay before teleport to tied enemy

        playerRefs.events.CallRespawn?.Invoke(); 

        playerRefs.healthSystem.RestoreAllHealth();

        onCutsceneOver?.Invoke();

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
