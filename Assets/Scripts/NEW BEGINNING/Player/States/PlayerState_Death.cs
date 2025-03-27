using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Death : PlayerState
{
    [SerializeField] XP_dropper xpDropper;
    [SerializeField] DeadPart_Instantiator deadPartsInstantiator;
    [SerializeField] GameState gameState;
    [SerializeField] GameObject DeathUIRoot;
    GameObject playerDeadHead;
    public override void OnEnable()
    {
        playerRefs.movement2.SetMovementSpeed(MovementSpeeds.Stopped);

        playerRefs.followMouse.UnfocusCurrentEnemy();

        gameState.playerDeaths++;

        // playerDeadHead = deadPartsInstantiator.InstantiateDeadParts(info)[0];


        playerRefs.hideSprites.HidePlayerSprites();

        

        spawnPlayerXps(); //Aixo es questionable

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
