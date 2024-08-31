using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SpecialAttack : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;
    [SerializeField] FloatVariable SpCharge_Max;
    [SerializeField] FloatVariable SpCharge_Current;
    [SerializeField] FloatVariable Health_Current;
    [SerializeField] FloatVariable Health_Max;
    [HideInInspector] public float Sp_Damage; //This is set from the weapon info holder
    [HideInInspector] public float Sp_Knockback;
    [HideInInspector] public float Sp_HitStop;
    [HideInInspector] public float StaminaCost;
    float amountToHeal;

    private void OnEnable()
    {
        InputDetector.Instance.OnSpecialAttackPressed += onSpecialAttackPressed;
        InputDetector.Instance.OnSpecialHealPressed += onSpecialHealPressed;
        playerRefs.events.OnDealtDamage += onAttackedEnemy;
        playerRefs.events.OnReceiveDamage += onReceivedAttack;
        playerRefs.events.OnPerformSpecialAttack += onPerformedSpecialAttack;
        playerRefs.events.OnSuccessfulParry += onSuccesfullParry;
        GameEvents.OnPlayerDeath += restartCharge;
        restartCharge();
    }
    private void OnDisable()
    {
        InputDetector.Instance.OnSpecialAttackPressed -= onSpecialAttackPressed;
        InputDetector.Instance.OnSpecialHealPressed -= onSpecialHealPressed;
        playerRefs.events.OnDealtDamage -= onAttackedEnemy;
        playerRefs.events.OnReceiveDamage -= onReceivedAttack;
        playerRefs.events.OnPerformSpecialAttack -= onPerformedSpecialAttack;
        playerRefs.events.OnSuccessfulParry -= onSuccesfullParry;
        GameEvents.OnPlayerDeath -= restartCharge;
    }
    void onSpecialAttackPressed()
    {
        if (SpCharge_Current.Value < SpCharge_Max.Value) { Debug.Log("not enough charge try again"); return; }

        playerRefs.actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Special01"));
    }
    void onSpecialHealPressed()
    {
        if (SpCharge_Current.Value < SpCharge_Max.Value) { Debug.Log("not enough charge try again"); return; }
        if(playerRefs.currentStamina.GetValue() == 0) { return; }
        playerRefs.actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Heal"));
        
        amountToHeal = Health_Max.Value - Health_Current.Value;
    }
    void onPerformedSpecialAttack()
    {
        SpCharge_Current.SetValue(0);
        playerRefs.events.OnStaminaAction(StaminaCost);
        foreach (Generic_DamageDealer dealer in playerRefs.DamageDealersList)
        {
            dealer.Damage = Sp_Damage * playerRefs.stats.DamageMultiplier;
            dealer.HitStop = Sp_HitStop;
            dealer.Knockback = Sp_Knockback;
            dealer.isChargingSpecialAttack = false;
        }
    }
    void onAttackedEnemy(object sender, Generic_EventSystem.DealtDamageInfo info)
    {
        addCharge(info.ChargeGiven);
    }
    void onReceivedAttack(object sender, Generic_EventSystem.ReceivedAttackInfo info)
    {
        addCharge(-info.Damage * .75f);
    }
    void onSuccesfullParry(object sender, Generic_EventSystem.SuccesfulParryInfo info)
    {
        addCharge(1.5f);
    }
    void addCharge(float amount)
    {
        float temporalValue = SpCharge_Current.Value + amount;
        
        if (temporalValue > SpCharge_Max.Value) { temporalValue = SpCharge_Max.Value; }
        else if (temporalValue < 0) { temporalValue = 0; }

        SpCharge_Current.SetValue(temporalValue);
    }
    void restartCharge()
    {
        SpCharge_Current.SetValue(0);
    }
    public void EV_ActuallyHeal()
    {
        //Health_Current.SetValue(Health_Current.Value + amountToHeal);
        restartCharge();
        playerRefs.healthSystem.RemoveLife(-amountToHeal, gameObject);
        playerRefs.events.OnActuallySpecialHeal?.Invoke();
    }
}
