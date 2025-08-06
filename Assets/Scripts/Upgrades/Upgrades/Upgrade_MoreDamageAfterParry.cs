
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "Upgrades/DamageAfterParry")]
public class Upgrade_MoreDamageAfterParry : Upgrade
{
    Player_References playerRefs;
    IParryDealer parryDealer;
    IDamageDealer damageDealer;
    [SerializeField] float DamageMultiplierAdded;
    [SerializeField] float UpgradeDurationAfterParry;
    [SerializeField] Color SwordColor = Color.cyan;
    [SerializeField] VisualEffectAsset VFX_AttackEnemyWithEffect;
    [SerializeField] Gradient SwordTrailGradiant;
    Gradient defaultGradiant;
    string CoroutineID = "afterParryDMG";
    bool isUnderEffect = false;
    public override void onAdded(GameObject entity)
    {
        
        playerRefs = entity.GetComponent<Player_References>();
        parryDealer = entity.GetComponent<IParryDealer>();
        damageDealer = entity.GetComponent<IDamageDealer>();

        parryDealer.OnParryDealt_event += OnParryDealt;
        damageDealer.OnDamageDealt_event += OnDamageDealt;
        defaultGradiant = playerRefs.weaponTrail.colorGradient;
    }

    public override void onRemoved(GameObject entity)
    {
        parryDealer.OnParryDealt_event -= OnParryDealt;
        damageDealer.OnDamageDealt_event -= OnDamageDealt;
        RemoveEffect();
    }
    private void OnParryDealt(SuccesfulParryInfo info)
    {
        CoroutinesRunner.instance.EndCoroutine(CoroutineID);
        CoroutinesRunner.instance.RunCoroutine(CoEffectCoroutine(), CoroutineID);
    }
    private void OnDamageDealt(DealtDamageInfo info)
    {
        if(isUnderEffect)
        {
            simpleVfxPlayer.Instance.playCustomVFX(VFX_AttackEnemyWithEffect, info.CollisionPosition);
        }
        CoroutinesRunner.instance.EndCoroutine(CoroutineID);
        RemoveEffect();
    }

    IEnumerator CoEffectCoroutine()
    {
        AddEffect();
        float timer = 0;
        while (timer < UpgradeDurationAfterParry)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        RemoveEffect();
    }
    void AddEffect()
    {
        if (isUnderEffect) { return; }
        isUnderEffect = true;
        playerRefs.weaponTrail.colorGradient = SwordTrailGradiant;
        playerRefs.weaponFlasher.StartFlashing(.1f, SwordColor);
        playerRefs.currentStats.DamageMultiplicator += DamageMultiplierAdded;
        
    }
    void RemoveEffect()
    {
        if(!isUnderEffect) { return; }
        isUnderEffect = false;
        playerRefs.weaponTrail.colorGradient = defaultGradiant;
        playerRefs.weaponFlasher.EndFlashing(.1f);
        playerRefs.currentStats.DamageMultiplicator -= DamageMultiplierAdded;
    }
    public override string shortDescription()
    {
        return $"Deal extra damage after a succesful {UsefullMethods.highlightString("Parry")}";
    }
}
