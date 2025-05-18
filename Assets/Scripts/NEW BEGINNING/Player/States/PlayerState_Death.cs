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

        playerRefs.movement.SetMovementSpeed(SpeedsEnum.Stopped);
        playerRefs.swordRotation.SetRotationSpeed(SpeedsEnum.Stopped);

        playerRefs.swordRotation.UnfocusCurrentEnemy();

        gameState.playerDeaths++;

        playerDeadHead = deadPartsInstantiator.InstantiateDeadParts()[0];

        playerRefs.hideSprites.HidePlayerSprites();

        //playerSinglePointCollider.enabled = false;

        //spawnPlayerXps();

       
        StartCoroutine(delayAndShowUI());

        //Wait and show UI
            //Change state to respawning?
            //Or load new scene?
    }
    IEnumerator delayAndShowUI()
    {
        yield return new WaitForSeconds(3);
        yield return playerDeadHead.GetComponent<PlayerHead_RebornBegin>().FlyAwayCoroutine();
        GameController.Instance.RespawnAfterDeath();
        yield break;
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
    public void Button_RespawnPlayer() //Try again. reload respawn room and respawning state
    {
      //no buttons for now

    }
    public void Button_RestartRun()//Return to main menu
    {
        //Load scene
        //Restart everything
        //DontDestroy needs to be tested
        DontDestroyOnLoad(rootGameObject);
        //Respawn in altomando index? Or should the altomando initializer manage it?
    }
}
