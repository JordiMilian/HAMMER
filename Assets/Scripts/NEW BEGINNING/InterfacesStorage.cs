using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfacesStorage : MonoBehaviour
{
}
#region INFO STRUCTS
public struct ReceivedAttackInfo
{
    public Vector3 CollisionPosition;
    public Vector2 GeneralDirection;
    public Vector2 ConcreteDirection;
    public GameObject Attacker;
    public float Damage;
    public float KnockBack;
    public float Hitstop;
    public bool IsBloody;
    public ReceivedAttackInfo(Vector2 collisionPosition,
        Vector2 Gdirection, Vector2 Cdirection, GameObject attacker, float damage, float knockBack, float hitstop, bool isBloody)
    {
        CollisionPosition = collisionPosition;
        GeneralDirection = Gdirection;
        ConcreteDirection = Cdirection;
        Attacker = attacker;
        Damage = damage;
        KnockBack = knockBack;
        Hitstop = hitstop;
        IsBloody = isBloody;
    }
}
public struct DealtDamageInfo
{
    public Vector3 CollisionPosition;
    public float DamageDealt;
    public float ChargeGiven;
    public GameObject AttackedRoot;
    public Generic_DamageDetector OtherDetector;
    public DealtDamageInfo(Vector3 collisionPosition, GameObject attacked, float damageDealt, Generic_DamageDetector otherDetector, float chargeGiven = 0)
    {
        CollisionPosition = collisionPosition;
        DamageDealt = damageDealt;
        ChargeGiven = chargeGiven;
        AttackedRoot = attacked;
        OtherDetector = otherDetector;
    }
}
public struct GettingParriedInfo
{
    public GameObject Parrier;
    public int WeaponIndex;
    public Vector2 ParryDirection;
    public GettingParriedInfo(GameObject parrier, int weaponIndex, Vector2 direction)
    {
        Parrier = parrier;
        WeaponIndex = weaponIndex;
        ParryDirection = direction;
    }
}
public struct SuccesfulParryInfo
{
    public Vector3 ParryPosition;
    public Generic_DamageDealer ParriedDamageDealer;
    public bool canChargeSpecialAttack;
    public SuccesfulParryInfo(Vector3 data, Generic_DamageDealer parriedDealer, bool canCharge)
    {
        ParryPosition = data;
        ParriedDamageDealer = parriedDealer;
        canChargeSpecialAttack = canCharge;
    }
}
public struct DeadCharacterInfo
{
    public GameObject DeadGameObject;
    public GameObject Killer;
    public IDamageDealer damageDealer;
    public DeadCharacterInfo(GameObject deadGameObject, GameObject killer, IDamageDealer damageD)
    {
        DeadGameObject = deadGameObject;
        Killer = killer;
        damageDealer = damageD;
    }
}
#endregion
#region INTERFACES
public interface IDamageReceiver
{
    public void OnDamageReceived(ReceivedAttackInfo info);
}
public interface IDamageDealer
{
    public void OnDamageDealt(DealtDamageInfo info);
}
public interface IParryReceiver
{
    public void OnParryReceived(GettingParriedInfo info);
}
public interface IParryDealer
{
    public void OnParryDealt(SuccesfulParryInfo info);
}
public interface IFocuseable
{
    public void OnFocused();
    public void OnUnfocused();
}
public interface IHealth
{
    public void RemoveHealth(float health);
    public void OnZeroHealth();
    public void RestoreAllHealth();
    public float GetCurrentHealth();
    public float GetMaxHealth();
}
public interface IStats
{
    public EntityStats GetCurrentStats();
    public void SetCurrentStats(EntityStats stats);
    public EntityStats GetBaseStats();

    public void SetBaseStats(EntityStats stats);

}
public interface IChangeStateByType
{
    public void ChangeStateByType(StateTags stateTag);
}
#endregion