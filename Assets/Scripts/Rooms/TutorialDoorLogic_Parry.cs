using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoorLogic_Parry : BaseRoomWithDoorLogic
{
    Player_EventSystem playerEvents;
    int parriesDone;
    [SerializeField] int parriesToOpen;
    [SerializeField] Generic_DamageDealer MannequinDamageDealer;
    [SerializeField] IParryReceiver ManequiinParryReceiver;
    [SerializeField] GameObject Manequiin;
    private void OnValidate()
    {
        if(ManequiinParryReceiver != null)
        {
            UsefullMethods.CheckIfGameobjectImplementsInterface<IParryReceiver>(ref Manequiin, ref ManequiinParryReceiver);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        ManequiinParryReceiver.OnParryReceived_event += Count1Parry;
    }
    void Count1Parry(GettingParriedInfo info)
    {
        parriesDone++;
        if (parriesDone == parriesToOpen)
        {
            RoomCompleted(true, true);
            ManequiinParryReceiver.OnParryReceived_event -= Count1Parry;
        }

    }
}
