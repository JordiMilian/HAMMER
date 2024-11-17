using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ExperienceCollector : MonoBehaviour
{
    Player_References playerRefs;
    private void Awake()
    {
        playerRefs = GetComponent<Player_References>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        XP_script xpScript = collision.GetComponent<XP_script>();
        if(xpScript != null )
        {
            playerRefs.gameState.xpPoints += xpScript.XpAmount;
            xpScript.onPickedUp();
        }
    }
}
