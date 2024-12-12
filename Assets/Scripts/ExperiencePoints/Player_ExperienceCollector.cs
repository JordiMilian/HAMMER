using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ExperienceCollector : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        XP_script script = collision.GetComponent<XP_script>();
        if(script != null )
        {
            playerRefs.currentStats.ExperiencePoints += script.XpAmount;
            //Debug.Log("Xp to add: " + script.XpAmount + " Total is: " + playerRefs.currentStats.ExperiencePoints);
            script.onPickedUp();
        }
    }
}
