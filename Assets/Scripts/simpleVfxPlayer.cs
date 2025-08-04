using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class simpleVfxPlayer : MonoBehaviour
{
    [SerializeField] Transform EffectorTF;
    [SerializeField] VisualEffect HitEnemyVFX;
    [SerializeField] VisualEffect HitEnemyParryVFX;
    [SerializeField] VisualEffect HitPlayerVFX;
    [SerializeField] VisualEffect BloodExplosionVFX;
    [SerializeField] VisualEffect StanceBrokenVFX;
    [SerializeField] VisualEffect BigPuddleStepVFX;
    [SerializeField] VisualEffect CenteredGroundSplashVFX;
    [SerializeField] VisualEffect CustomEffect;
    [SerializeField] Dictionary<simpleVFXkeys,VisualEffect> vfxDictionary = new Dictionary<simpleVFXkeys, VisualEffect>();

    public static simpleVfxPlayer Instance;
    public enum simpleVFXkeys
    { 
        HitEnemy,
        HitEnemyParry,
        HitPlayer,
        BloodExplosion,
        StanceBroken,
        BigPuddleStep
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        vfxDictionary[simpleVFXkeys.HitEnemy] = HitEnemyVFX;
        vfxDictionary[simpleVFXkeys.HitEnemyParry] = HitEnemyParryVFX;
        vfxDictionary[simpleVFXkeys.HitPlayer] = HitPlayerVFX;
        vfxDictionary[simpleVFXkeys.BloodExplosion] = BloodExplosionVFX;
        vfxDictionary[simpleVFXkeys.StanceBroken] = StanceBrokenVFX;
        vfxDictionary[simpleVFXkeys.BigPuddleStep] = BigPuddleStepVFX;
    }
    public void playSimpleVFX(simpleVFXkeys vfxKey, Vector2 position)
    {
        if(vfxDictionary.ContainsKey(vfxKey))
        {
            EffectorTF.position = position;
            vfxDictionary[vfxKey].Play();
        }
        else
        {
            Debug.LogWarning("Dictionary does not contain effect: "+ vfxKey);
        }
    }
    public void playCustomVFX(VisualEffectAsset effect, Vector2 position)
    {
        CustomEffect.visualEffectAsset = effect;
        EffectorTF.position = position;
        CustomEffect.Play();
    }

}
