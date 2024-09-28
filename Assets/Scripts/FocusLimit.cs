using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusLimit : MonoBehaviour
{
    [SerializeField] GameObject FocuseTarget;
    Player_FollowMouse_alwaysFocus playerFocus;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerFocus == null) { playerFocus = GlobalPlayerReferences.Instance.references.followMouse; }

        if(collision.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            if(playerFocus.FocusedEnemy == FocuseTarget)
            {
                playerFocus.OnLookAtMouse(FocuseTarget);
            }
        }
    }
}
