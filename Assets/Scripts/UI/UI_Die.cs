using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Die : UI_BaseAction
{
    public override void Action(UI_Button button)
    {
        //Find the player and kill it
        IDamageReceiver playerReceiver = GlobalPlayerReferences.Instance.playerTf.gameObject.GetComponent<IDamageReceiver>();
        playerReceiver.OnDamageReceived( new ReceivedAttackInfo(
            Vector2.zero,
            Vector2.zero,
            Vector2.zero,
            gameObject,
            null,
            100,
            0,
            false
            ));

        //find the pauseGame script in root and unpause
        button.transform.root.GetComponent<PauseGame>().UnpauseGame();
    }
}
