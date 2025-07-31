using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyRoom_Controller : MonoBehaviour, IRoom, IMultipleRoom
{
    #region MULTIPLE ROOM
    [SerializeField] Transform _tf_ExitPos;
    public Vector2 ExitPos => _tf_ExitPos.position;

    [SerializeField] Generic_OnTriggerEnterEvents _combinedCollider;
    public Generic_OnTriggerEnterEvents combinedCollider => _combinedCollider;

    #endregion

    public void OnRoomLoaded()
    {
        SetPoleText();
    }

    public void OnRoomUnloaded()
    {
    }
    [SerializeField] Dialoguer dialoguer_Attack;
    void SetPoleText()
    {
        dialoguer_Attack.TextLines = new List<string>()
        {
            $"Press <color=red>{InputDetector.Instance.Attack_String()}<color=black> to Attack",
            "Attack this Pole to keet reading",
            "Awesome, you can continue"

        };
    }
}
