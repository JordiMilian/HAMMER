using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom_SpecialAttack_Controller : MonoBehaviour, IRoom, IMultipleRoom
{
    [SerializeField] Transform _tf_ExitPos;
    public Vector2 ExitPos => _tf_ExitPos.position;

    [SerializeField] Generic_OnTriggerEnterEvents _combinedCollider;
    public Generic_OnTriggerEnterEvents combinedCollider => _combinedCollider;

    public void OnRoomLoaded()
    {
    }

    public void OnRoomUnloaded()
    {
    }
}
