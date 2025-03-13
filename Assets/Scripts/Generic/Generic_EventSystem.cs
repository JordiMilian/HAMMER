using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_EventSystem : MonoBehaviour
{
    public class ObjectDirectionArgs : EventArgs
    {
        public Vector2 GeneralDirection;
        
        public ObjectDirectionArgs(Vector2 Gdirection)
        {
            GeneralDirection = Gdirection;
            
        }
    }

    public EventHandler<DeadCharacterInfo> OnDeath;
    public Action OnAttackFinished; //On enemies this is called after waiting for the animation time. On Player it is called from the animator on exit state
    public EventHandler<DealtDamageInfo> OnDealtDamage;
    public EventHandler<ReceivedAttackInfo> OnReceiveDamage;
    public Action<GettingParriedInfo> OnGettingParried;
    public EventHandler<SuccesfulParryInfo> OnSuccessfulParry;
    public EventHandler<DealtDamageInfo> OnHitObject;
    public EventHandler<ObjectDirectionArgs> OnBeingTouchedObject;
}
