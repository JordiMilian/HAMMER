using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom_Attack_Controller : MonoBehaviour, IRoom, IMultipleRoom
{
    [SerializeField] DoorAnimationController exitDoorController;
    Player_References playerRefs;
    bool performedStrong, performedQuick;

    #region MULTIPLE ROOM
    [SerializeField] Transform _tf_ExitPos;
    public Vector2 ExitPos => _tf_ExitPos.position;

    [SerializeField] Generic_OnTriggerEnterEvents _combinedCollider;
    public Generic_OnTriggerEnterEvents combinedCollider => _combinedCollider;
    #endregion
    public void OnRoomLoaded()
    {
        playerRefs = GlobalPlayerReferences.Instance.references;
        playerRefs.stateMachine.OnStateChanged += OnStateChanged;
    }
    void OnStateChanged(PlayerState newState)
    {
        if(newState == playerRefs.GestureAttack_Strong)
        {
             performedStrong = true;
        }
        if(newState == playerRefs.StartingComboAttackState)
        {
            performedQuick = true;
        }
        if(performedQuick && performedStrong)
        {
            playerRefs.stateMachine.OnStateChanged -= OnStateChanged;
            Invoke("OpenDoorWithDelay", .5f);
        }
    }
    void OpenDoorWithDelay() 
    { 
        exitDoorController.OpenWithCutscene();
    }
    public void OnRoomUnloaded()
    {
        
    }
}
