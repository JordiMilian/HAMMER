using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom_Roll_Controller : MonoBehaviour, IRoom, IMultipleRoom
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
    [SerializeField] Dialoguer dialoguer_Roll, dialoguer_Attack;
    private void Start()
    {
        SetPoleText();
    }
    void SetPoleText()
    {
        dialoguer_Roll.TextLines = new List<string>()
        {
            $"Press <color=red>{InputDetector.Instance.Roll_string()}<color=black> to Roll",
            //"<color=red>Hold LT<color=black> to Sprint",
            "<color=red>Roll<color=black> over the spikes to continue"
        };
        dialoguer_Attack.TextLines = new List<string>()
        {
            $"Press <color=red>{InputDetector.Instance.Attack_String()}<color=black> to Attack",
            "Attack this Pole to keet reading",
            "Awesome, you can continue"

        };
    }
}
