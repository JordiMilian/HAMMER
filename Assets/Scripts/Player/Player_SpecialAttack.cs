using UnityEngine;

public class Player_SpecialAttack : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;

    [HideInInspector] public float Sp_Damage; //This is set from the weapon info holder
    [HideInInspector] public float Sp_Knockback;
    [HideInInspector] public float Sp_HitStop;
    [HideInInspector] public float StaminaCost;
    float amountToHeal;
    PlayerStats stats;

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
        stats = playerRefs.currentStats;
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
        if (stats.CurrentBloodFlow < stats.MaxBloodFlow) { Debug.Log("not enough charge try again"); return; }

        playerRefs.actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Special01"));
    }
    void onSpecialHealPressed()
    {
        if (stats.CurrentBloodFlow < stats.MaxBloodFlow) { Debug.Log("not enough charge try again"); return; }
        if(playerRefs.currentStats.CurrentStamina <= 0) { return; }
        playerRefs.actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Heal"));
        
        amountToHeal = stats.MaxHp - stats.CurrentHp;
    }
    void onPerformedSpecialAttack()
    {
        stats.CurrentBloodFlow = 0;
        playerRefs.events.CallStaminaAction(StaminaCost);
        foreach (Generic_DamageDealer dealer in playerRefs.DamageDealersList)
        {
            dealer.Damage = Sp_Damage * playerRefs.currentStats.DamageMultiplicator;
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
        //addCharge(-info.Damage * .75f); TESTING NOT REMOVE SPECIAL
    }
    void onSuccesfullParry(object sender, Generic_EventSystem.SuccesfulParryInfo info)
    {
        if (info.canChargeSpecialAttack) { addCharge(1.25f); }
        
    }
    void addCharge(float amount)
    {
        float temporalValue = stats.CurrentBloodFlow + (amount * stats.BloodflowMultiplier);
        
        if (temporalValue > stats.MaxBloodFlow) { temporalValue = stats.MaxBloodFlow; }
        else if (temporalValue < 0) { temporalValue = 0; }

        stats.CurrentBloodFlow =temporalValue;
    }
    void restartCharge()
    {
        stats.CurrentBloodFlow = 0;
    }
    public void EV_ActuallyHeal()
    {
        //Health_Current.SetValue(Health_Current.Value + amountToHeal);
        restartCharge();
        playerRefs.healthSystem.RemoveLife(-amountToHeal, gameObject);
        playerRefs.events.OnActuallySpecialHeal?.Invoke();
    }
}
