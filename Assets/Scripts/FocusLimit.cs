using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusLimit : MonoBehaviour
{
    [SerializeField] Focuseable FocuseTarget;
    Player_SwordRotationController playerFocus;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerFocus == null) { playerFocus = GlobalPlayerReferences.Instance.references.swordRotation; }

        if(collision.CompareTag(Tags.Player_SinglePointCollider))
        {
            if(playerFocus.CurrentFocuseable == FocuseTarget)
            {
                playerFocus.UnfocusCurrentEnemy();
            }
        }
    }
}
