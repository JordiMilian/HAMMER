using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_DeathController : MonoBehaviour
{
    [SerializeField] Transform DeathUIRoot;
    [SerializeField] Player_References playerRefs;
    [SerializeField] GameState gameState;
    GameObject playerDeadHead;
    private void OnEnable()
    {
        playerRefs.events.OnDeath += StartDeath;
        DeathUIRoot.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        playerRefs.events.OnDeath -= StartDeath;
    }
    void StartDeath(object sender, Generic_EventSystem.DeadCharacterInfo info)
    {
        gameState.playerDeaths++;
        playerRefs.events.CallHideAndDisable();
        DeadPart_Instantiator_player deadPartInst = playerRefs.gameObject.GetComponent<DeadPart_Instantiator_player>();
        playerDeadHead = deadPartInst.InstantiateDeadParts(this, info)[0];

        StartCoroutine(startDeathCoroutine());
    }
    IEnumerator startDeathCoroutine()
    {
        yield return new WaitForSeconds(3);
        DeathUIRoot.gameObject.SetActive(true);
    }

    //Called from UI
    public void SpawnAgain()
    {
        DeathUIRoot.gameObject.SetActive(false);
        StartCoroutine(RespawnCoroutine());
    }
    IEnumerator RespawnCoroutine()
    {
        yield return StartCoroutine( playerDeadHead.GetComponent<PlayerHead_RebornBegin>().FlyAwayCoroutine());
        SetupForRespawn();

        RespawnersManager.Instance.RespawnPlayer();
        playerRefs.healthSystem.RestoreAllHealth();

        //
        void SetupForRespawn()
        {
            Transform weaponPivot =playerRefs.weaponPivot.transform;

            weaponPivot.transform.eulerAngles = new Vector3(
                        weaponPivot.transform.eulerAngles.x,
                        weaponPivot.transform.eulerAngles.y,
                        90
                        );
        }
    }
    public void EndRun()
    {
        DeathUIRoot.gameObject.SetActive(false);
        SceneManager.LoadScene("OutOfRunWorld");
    }
}
