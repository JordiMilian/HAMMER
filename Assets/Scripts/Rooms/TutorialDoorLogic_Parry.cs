using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoorLogic_Parry : BaseRoomWithDoorLogic
{
    int parriesDone;
    [SerializeField] int parriesToOpen;
     IParryReceiver Manequin_IParryReceiver;
    [SerializeField] GameObject ManequinWithParryReceiver;
    private void OnValidate()
    {
        if(ManequinWithParryReceiver != null)
        {
            UsefullMethods.CheckIfGameobjectImplementsInterface<IParryReceiver>(ref ManequinWithParryReceiver, ref Manequin_IParryReceiver);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        OnValidate();
        Manequin_IParryReceiver.OnParryReceived_event += Count1Parry;
    }
    void Count1Parry(GettingParriedInfo info)
    {
        parriesDone++;
        if (parriesDone == parriesToOpen)
        {
            RoomCompleted(true, true);
            Manequin_IParryReceiver.OnParryReceived_event -= Count1Parry;
        }

    }
}
