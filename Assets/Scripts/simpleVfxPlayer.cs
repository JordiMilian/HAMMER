using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class simpleVfxPlayer : MonoBehaviour
{
    [SerializeField] Transform PlayerTF;
    [SerializeField] VisualEffect HitEnemyVFX;
    [SerializeField] VisualEffect HitEnemyParryVFX;
    [SerializeField] VisualEffect HitPlayerVFX;
    [SerializeField] VisualEffect BloodExplosionVFX;
    [SerializeField] VisualEffect StanceBrokenVFX;
    [SerializeField] Dictionary<simpleVFX,VisualEffect> vfxDictionary = new Dictionary<simpleVFX, VisualEffect>();

    public static simpleVfxPlayer Instance;
    public enum simpleVFX
    { 
        HitEnemy,
        HitEnemyParry,
        HitPlayer,
        BloodExplosion,
        StanceBroken,
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

        vfxDictionary[simpleVFX.HitEnemy] = HitEnemyVFX;
        vfxDictionary[simpleVFX.HitEnemyParry] = HitEnemyParryVFX;
        vfxDictionary[simpleVFX.HitPlayer] = HitPlayerVFX;
        vfxDictionary[simpleVFX.BloodExplosion] = BloodExplosionVFX;
        vfxDictionary[simpleVFX.StanceBroken] = StanceBrokenVFX;
    }

    public void playSimpleVFX(simpleVFX vfxKey, Vector2 position)
    {
        if(vfxDictionary.ContainsKey(vfxKey))
        {
            PlayerTF.position = position;
            vfxDictionary[vfxKey].Play();
        }
        else
        {
            Debug.LogWarning("Dictionary does not contain effect: "+ vfxKey);
        }
    }
    
}
