using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Death : PlayerState
{
    [SerializeField] XP_dropper xpDropper;
    [SerializeField] DeadPart_Instantiator deadPartsInstantiator;
    [SerializeField] GameState gameState;
    [SerializeField] GameObject DeathUIRoot;
    [SerializeField] Collider2D playerMainCollider;
    GameObject playerDeadHead;
    public override void OnEnable()
    {
        TargetGroupSingleton.Instance.RemovePlayersTarget();

        playerRefs.movement2.SetMovementSpeed(MovementSpeeds.Stopped);

        playerRefs.followMouse.UnfocusCurrentEnemy();

        gameState.playerDeaths++;

        playerDeadHead = deadPartsInstantiator.InstantiateDeadParts()[0];


        playerRefs.hideSprites.HidePlayerSprites();

        playerMainCollider.enabled = false;

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
        playerMainCollider.enabled = true;
    }
    public void Button_RespawnPlayer()
    {
        //deadHead go up
        //delay
        //Change state to respawn
    }
    public void Button_RestartRun()
    {
        //Load scene
    }
}
