using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusLimit : MonoBehaviour
{
    [SerializeField] GameObject FocuseTarget;
    [SerializeField] Player_FollowMouse_withFocus playerFocus;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            if(playerFocus.FocusedEnemy == FocuseTarget)
            {
                playerFocus.OnLookAtMouse(FocuseTarget);
            }
        }
    }
}
