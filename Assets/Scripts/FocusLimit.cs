using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusLimit : MonoBehaviour
{
    [SerializeField] GameObject FocuseTarget;
    Player_FollowMouseWithFocus_V2 playerFocus;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerFocus == null) { playerFocus = GlobalPlayerReferences.Instance.references.followMouse; }

        if(collision.CompareTag(Tags.Player_SinglePointCollider))
        {
            if(playerFocus.CurrentlyFocusedEnemy == FocuseTarget)
            {
                playerFocus.UnfocusCurrentEnemy();
            }
        }
    }
}
