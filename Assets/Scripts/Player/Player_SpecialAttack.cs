using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SpecialAttack : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;
    [SerializeField] FloatVariable SpCharge_Max;
    [SerializeField] FloatVariable SpCharge_Current;
    [SerializeField] float Sp_Damage;
    [SerializeField] float Sp_Knockback;
    [SerializeField] float Sp_HitStop;

    private void OnEnable()
    {
        InputDetector.Instance.OnSpecialAttackPressed += onSpecialAttackPressed;
        playerRefs.events.OnDealtDamage += onAttackedEnemy;
        playerRefs.events.OnReceiveDamage += onReceivedAttack;
        playerRefs.events.OnPerformSpecialAttack += onPerformedSpecialAttack;
    }
    private void OnDisable()
    {
        InputDetector.Instance.OnSpecialAttackPressed -= onSpecialAttackPressed;
        playerRefs.events.OnDealtDamage -= onAttackedEnemy;
        playerRefs.events.OnReceiveDamage -= onReceivedAttack;
        playerRefs.events.OnPerformSpecialAttack -= onPerformedSpecialAttack;
    }
    void onSpecialAttackPressed()
    {
        if (SpCharge_Current.Value < SpCharge_Max.Value) { Debug.Log("not enough charge try again"); return; }

        playerRefs.actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Special01"));
    }
    void onPerformedSpecialAttack()
    {
        SpCharge_Current.Value = 0;
        playerRefs.damageDealer.Damage = Sp_Damage;
        playerRefs.damageDealer.HitStop = Sp_HitStop;
        playerRefs.damageDealer.Knockback = Sp_Knockback;
        playerRefs.damageDealer.isChargingSpecialAttack = false;
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
        SpCharge_Current.Value += amount;
        if (SpCharge_Current.Value > SpCharge_Max.Value) { SpCharge_Current.Value = SpCharge_Max.Value; }
        else if (SpCharge_Current.Value < 0) { SpCharge_Current.Value = 0; }
    }
}
