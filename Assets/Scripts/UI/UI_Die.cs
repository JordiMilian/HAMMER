using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Die : UI_BaseAction
{
    public override void Action(UI_Button button)
    {
        //Find the player and kill it
        Player_EventSystem playerEvents = GameObject.Find(TagsCollection.MainCharacter).GetComponent<Player_EventSystem>();
        playerEvents.OnDeath?.Invoke(this, new Generic_EventSystem.DeadCharacterInfo(playerEvents.gameObject, gameObject));

        //find the pauseGame script in root and unpause
        button.transform.root.GetComponent<PauseGame>().Unpause();
    }
}
