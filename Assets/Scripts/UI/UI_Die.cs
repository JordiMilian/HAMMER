using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Die : UI_BaseAction
{
    public override void Action(UI_Button button)
    {
        //Find the player and kill it
        Player_EventSystem playerEvents = GlobalPlayerReferences.Instance.references.events;
        playerEvents.OnReceiveDamage(this, new Generic_EventSystem.ReceivedAttackInfo(
            Vector2.zero,
            Vector2.zero,
            Vector2.zero,
            gameObject,
            100,
            0,
            0,
            false
            ));

        //find the pauseGame script in root and unpause
        button.transform.root.GetComponent<PauseGame>().Unpause();
    }
}
