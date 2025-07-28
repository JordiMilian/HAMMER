
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Upgrade_MoreDamageAfterParry : Upgrade
{
    Player_References playerRefs;
    IParryDealer parryDealer;
    IDamageDealer damageDealer;
    bool isUnderEffect;
    [SerializeField] float DamageMultiplierAdded;
    [SerializeField] float UpgradeDurationAfterParry;
    string CoroutineID = "afterParryDMG";
    public override void onAdded(GameObject entity)
    {
        playerRefs = entity.GetComponent<Player_References>();
        parryDealer = entity.GetComponent<IParryDealer>();
        damageDealer = entity.GetComponent<IDamageDealer>();

        parryDealer.OnParryDealt_event += OnParryDealt;
        damageDealer.OnDamageDealt_event += OnDamageDealt;
    }

    public override void onRemoved(GameObject entity)
    {
        parryDealer.OnParryDealt_event -= OnParryDealt;
        damageDealer.OnDamageDealt_event -= OnDamageDealt;
    }
    private void OnParryDealt(SuccesfulParryInfo info)
    {
        CoroutinesRunner.instance.EndCoroutine(CoroutineID);
        CoroutinesRunner.instance.RunCoroutine(EffectCoroutine(), CoroutineID);
    }
    private void OnDamageDealt(DealtDamageInfo info)
    {
        CoroutinesRunner.instance.EndCoroutine(CoroutineID);
        RemoveEffect();
    }

    IEnumerator EffectCoroutine()
    {
        AddEffect();
        float timer = 0;
        while (timer < 2f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        RemoveEffect();
    }
    void AddEffect()
    {
        //TO DO: Add visual feedback
        playerRefs.weaponScalingRoot.transform.localScale *= -1;
        playerRefs.currentStats.DamageMultiplicator += DamageMultiplierAdded;
        isUnderEffect = true;
    }
    void RemoveEffect()
    {
        playerRefs.weaponScalingRoot.transform.localScale *= -1;
        playerRefs.currentStats.DamageMultiplicator -= DamageMultiplierAdded;
        isUnderEffect = false;
    }
    public override string shortDescription()
    {
        return "Attacking after a succesfull parry deals extra damage";
    }
}
