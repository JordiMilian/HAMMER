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
        DeathUIRoot.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
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
        playerRefs.GetComponent<IHealth>().RestoreAllHealth();

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
