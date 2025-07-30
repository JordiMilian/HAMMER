using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom_Parry_Controller : MonoBehaviour, IRoom, IMultipleRoom
{
    int parriesDone;
    [SerializeField] int parriesToOpen;
     IParryReceiver Manequin_IParryReceiver;
    [SerializeField] GameObject ManequinWithParryReceiver;
    [SerializeField] DoorAnimationController exitDoorController;
    private void OnValidate()
    {
        if(ManequinWithParryReceiver != null)
        {
            UsefullMethods.CheckIfGameobjectImplementsInterface<IParryReceiver>(ref ManequinWithParryReceiver, ref Manequin_IParryReceiver);
        }
    }
    public void OnRoomLoaded()
    {
        OnValidate();
        Manequin_IParryReceiver.OnParryReceived_event += Count1Parry;
        exitDoorController.DisableAutoDoorOpener();
    }
    public void OnRoomUnloaded()
    {
        Manequin_IParryReceiver.OnParryReceived_event -= Count1Parry;
    }
    void Count1Parry(GettingParriedInfo info)
    {
        parriesDone++;
        if (parriesDone == parriesToOpen)
        {
            exitDoorController.OpenWithCutscene();
            Manequin_IParryReceiver.OnParryReceived_event -= Count1Parry;
        }
    }
    //This is for being part of a Multiple Room
    [SerializeField] Transform _tf_ExitPos;
    public Vector2 ExitPos => _tf_ExitPos.position;

    [SerializeField] Generic_OnTriggerEnterEvents _combinedCollider;
    public Generic_OnTriggerEnterEvents combinedCollider => _combinedCollider;

    [SerializeField] Dialoguer dialoguer;
    private void Start()
    {
        SetPoleText();
    }
    void SetPoleText()
    {
        dialoguer.TextLines = new List<string>()
        {
            $"Press <color=red>{InputDetector.Instance.Parry_String()}<color=black> to Parry",
            "<color=red>Parry<color=black> the enemy to continue"
        };
    }
}
