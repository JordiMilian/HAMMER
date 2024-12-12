using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP_PlayerDropper : XP_dropper
{
    [SerializeField] Player_References playerRefs;
    Player_EventSystem playerEvents;
    private void OnEnable()
    {
        playerEvents = playerRefs.events;
        playerEvents.OnDeath += spawnPlayerXps;
    }
    private void OnDisable()
    {
        playerEvents.OnDeath -= spawnPlayerXps;
    }

    void spawnPlayerXps(object sender, Generic_EventSystem.DeadCharacterInfo info)
    {
        GameObject[] playersSpawnedXp = spawnExperiences(playerRefs.currentStats.ExperiencePoints);
        foreach(GameObject xp in playersSpawnedXp)
        {
            xp.GetComponent<XP_script>().OnPlayerSpawn();
            xp.GetComponent<CircleCollider2D>().enabled = false;
            xp.GetComponentInChildren<XP_MoveTowardsPlayer>().enabled = false;
        }
        playerRefs.currentStats.ExperiencePoints = 0;
    }
}
