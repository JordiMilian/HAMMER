using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Generic_DamageDealer;
using static Player_ParryPerformer;

public class VFX_AttacksSpawner : MonoBehaviour
{
    [SerializeField] Player_ParryPerformer SuccesfullParryDetector;
    [SerializeField] Generic_DamageDealer damageDealer;
    [SerializeField] Generic_DamageDetector damageDetector;

    [SerializeField] GameObject VFX_Parry;
    [SerializeField] GameObject VFX_HitEnemy;
    [SerializeField] GameObject VFX_HitObject;
    [SerializeField] GameObject VFX_ReceiveDamage;
    private void OnEnable()
    {
        SuccesfullParryDetector.OnSuccessfulParry += InstantiateParryVFX;
        damageDealer.OnDealtDamage += InstantiateDealDamageVFX;
    }
    private void OnDisable()
    {
        SuccesfullParryDetector.OnSuccessfulParry -= InstantiateParryVFX;
        damageDealer.OnDealtDamage -= InstantiateDealDamageVFX;
    }
    public void InstantiateParryVFX(object sender, EventArgs_ParryInfo parryInfo)
    {
        Instantiate(VFX_Parry, parryInfo.vector3data, Quaternion.identity);
    }
   
    void InstantiateDealDamageVFX(object sender, EventArgs_DealtDamageInfo dealtDamageInfo)
    {
        Instantiate(VFX_HitEnemy,dealtDamageInfo.CollisionPosition, Quaternion.identity);
    }
}
