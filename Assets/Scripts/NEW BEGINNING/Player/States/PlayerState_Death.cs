using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState_Death : PlayerState
{
    [SerializeField] XP_dropper xpDropper;
    [SerializeField] DeadPart_Instantiator deadPartsInstantiator;
    [SerializeField] GameState gameState;
    [SerializeField] GameObject DeathUIRoot;
    [SerializeField] Collider2D playerSinglePointCollider;
    GameObject playerDeadHead;
    public override void OnEnable()
    {
        TargetGroupSingleton.Instance.RemovePlayersTarget();

        playerRefs.movement2.SetMovementSpeed(SpeedsEnum.Stopped);

        playerRefs.swordRotation.UnfocusCurrentEnemy();

        gameState.playerDeaths++;

        playerDeadHead = deadPartsInstantiator.InstantiateDeadParts()[0];


        playerRefs.hideSprites.HidePlayerSprites();

        playerSinglePointCollider.enabled = false;

        spawnPlayerXps(); //Aixo es questionable, consultar amb diseny

       
        StartCoroutine(delayAndShowUI());

        //Wait and show UI
            //Change state to respawning?
            //Or load new scene?
    }
    IEnumerator delayAndShowUI()
    {
        yield return new WaitForSeconds(3);
        DeathUIRoot.SetActive(true);
    }
    void spawnPlayerXps()
    {
        GameObject[] playersSpawnedXp = xpDropper.spawnExperiences(playerRefs.currentStats.ExperiencePoints);
        foreach (GameObject xp in playersSpawnedXp)
        {
            xp.GetComponent<XP_script>().OnPlayerSpawn();
            xp.GetComponent<CircleCollider2D>().enabled = false;
            xp.GetComponentInChildren<XP_MoveTowardsPlayer>().enabled = false;
        }
        playerRefs.currentStats.ExperiencePoints = 0;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        playerSinglePointCollider.enabled = true;
    }
    public void Button_RespawnPlayer()
    {
        StartCoroutine(respawnCoroutine());

        //
        IEnumerator respawnCoroutine()
        {
            yield return StartCoroutine(playerDeadHead.GetComponent<PlayerHead_RebornBegin>().FlyAwayCoroutine());
            stateMachine.ForceChangeState(playerRefs.RespawningState);
        }
    }
    public void Button_RestartRun()
    {
        //Load scene
        //Restart everything
        //DontDestroy needs to be tested
        DontDestroyOnLoad(rootGameObject);
        SceneManager.LoadScene("AltoMando");
        //Respawn in altomando index? Or should the altomando initializer manage it?
    }
}
