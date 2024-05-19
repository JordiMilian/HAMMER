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
    [SerializeField] float Sp_Damage;
    [SerializeField] float Sp_Knockback;
    [SerializeField] float Sp_HitStop;

    private void OnEnable()
    {
        InputDetector.Instance.OnSpecialAttackPressed += onSpecialAttackPressed;
        InputDetector.Instance.OnSpecialHealPressed += onSpecialHealPressed;
        playerRefs.events.OnDealtDamage += onAttackedEnemy;
        playerRefs.events.OnReceiveDamage += onReceivedAttack;
        playerRefs.events.OnPerformSpecialAttack += onPerformedSpecialAttack;
    }
    private void OnDisable()
    {
        InputDetector.Instance.OnSpecialAttackPressed -= onSpecialAttackPressed;
        InputDetector.Instance.OnSpecialHealPressed -= onSpecialHealPressed;
        playerRefs.events.OnDealtDamage -= onAttackedEnemy;
        playerRefs.events.OnReceiveDamage -= onReceivedAttack;
        playerRefs.events.OnPerformSpecialAttack -= onPerformedSpecialAttack;
    }
    void onSpecialAttackPressed()
    {
        if (SpCharge_Current.Value < SpCharge_Max.Value) { Debug.Log("not enough charge try again"); return; }

        playerRefs.actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Special01"));
    }
    void onSpecialHealPressed()
    {
        if (SpCharge_Current.Value < SpCharge_Max.Value) { Debug.Log("not enough charge try again"); return; }

        SpCharge_Current.SetValue(0);
        HealItself();
    }
    void onPerformedSpecialAttack()
    {
        SpCharge_Current.SetValue(0);
        playerRefs.damageDealer.Damage = Sp_Damage;
        playerRefs.damageDealer.HitStop = Sp_HitStop;
        playerRefs.damageDealer.Knockback = Sp_Knockback;
        playerRefs.damageDealer.isChargingSpecialAttack = false;
    }
    void HealItself()
    {
        Health_Current.SetValue(Health_Max.Value);
    }
    void onAttackedEnemy(object sender, Generic_EventSystem.DealtDamageInfo info)
    {
        addCharge(info.ChargeGiven);
    }
    void onReceivedAttack(object sender, Generic_EventSystem.ReceivedAttackInfo info)
    {
        addCharge(-info.Damage);
    }
    void addCharge(float amount)
    {
        float temporalValue = SpCharge_Current.Value + amount;
        
        if (temporalValue > SpCharge_Max.Value) { temporalValue = SpCharge_Max.Value; }
        else if (temporalValue < 0) { temporalValue = 0; }

        SpCharge_Current.SetValue(temporalValue);
    }
}
