using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfacesStorage : MonoBehaviour
{
}
public enum DamagersTeams
{
    Player,Enemy,Neutral
}
#region INFO STRUCTS
public struct ReceivedAttackInfo
{
    public Vector3 CollisionPosition;
    public Vector2 RootsDirection;
    public Vector2 CollidersDirection;
    public GameObject AttackerRoot_Go;
    public Generic_DamageDealer OtherDamageDealer;

    public float Damage;
    public float KnockBack;
    public bool IsBloody;
    public ReceivedAttackInfo(Vector2 collisionPosition,Vector2 rootsDirection, Vector2 colldiersDirection, 
        GameObject attackerRoot, Generic_DamageDealer dealer, float damage, float knockBack, bool isBloody)
    {
        CollisionPosition = collisionPosition;
        RootsDirection = rootsDirection;
        CollidersDirection = colldiersDirection;
        AttackerRoot_Go = attackerRoot;
        OtherDamageDealer = dealer;

        Damage = damage;
        KnockBack = knockBack;
        IsBloody = isBloody;
    }
}
public struct DealtDamageInfo
{
    public Vector3 CollisionPosition;
    public float DamageDealt;
    public float ChargeGiven;
    public GameObject AttackedRoot;
    public Generic_DamageDetector OtherDamageDetector;
    public DealtDamageInfo(Vector3 collisionPosition, GameObject attackerRoot, float damageDealt, Generic_DamageDetector otherDetector, float chargeGiven = 0)
    {
        CollisionPosition = collisionPosition;
        DamageDealt = damageDealt;
        ChargeGiven = chargeGiven;
        AttackedRoot = attackerRoot;
        OtherDamageDetector = otherDetector;
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
    public GameObject ParriedRootGameObject;
    public SuccesfulParryInfo(Vector3 position, Generic_DamageDealer parriedDealer, bool canCharge, GameObject parriedRoot)
    {
        ParryPosition = position;
        ParriedDamageDealer = parriedDealer;
        canChargeSpecialAttack = canCharge;
        ParriedRootGameObject = parriedRoot;
    }
}
public struct DeadCharacterInfo
{
    public GameObject DeadGameObject;
    public GameObject KillerRootGO;
    public Generic_DamageDealer damageDealer;
    public DeadCharacterInfo(GameObject deadGameObject, GameObject KillerRoot, Generic_DamageDealer damageD)
    {
        DeadGameObject = deadGameObject;
        KillerRootGO = KillerRoot;
        damageDealer = damageD;
    }
}
#endregion
#region INTERFACES
public interface IDamageReceiver
{
    public Action<ReceivedAttackInfo> OnDamageReceived_event { get; set; }
    public void OnDamageReceived(ReceivedAttackInfo info);

    
}
/* COPY AND PASTES THIS?
 
    public Action<ReceivedAttackInfo> OnDamageReceived_Event { get; set; }
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        OnDamageReceived_Event?.Invoke(info);
    }
*/
public interface IDamageDealer
{
    public void OnDamageDealt(DealtDamageInfo info);
    public Action<DealtDamageInfo> OnDamageDealt_event { get; set; }
}
public interface IParryReceiver
{
    public void OnParryReceived(GettingParriedInfo info);
    public Action<GettingParriedInfo> OnParryReceived_event { get; set; }
}
public interface IParryDealer
{
    public void OnParryDealt(SuccesfulParryInfo info);
    public Action<SuccesfulParryInfo> OnParryDealt_event { get; set; }
}
public interface IKilleable
{
    public Action <DeadCharacterInfo> OnKilled_event { get; set; }
    public void OnKilled(DeadCharacterInfo info);

}
public interface IHealth
{
    public void RemoveHealth(float health);
    public Action OnHealthUpdated {  get; set; }
    public float GetCurrentHealth();
    public float GetMaxHealth();
    public void RestoreAllHealth();
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
public interface IAttackedWhileRecovery
{
    public bool isInRecovery { get; set; }
    public void OnAttackedWhileRecovery();

}
public interface IAddForceStats
{
    public float DefaultOtherDistance { get; set; }
    public float MinOtherDistance { get; set; }
    public float MaxOtherDistance { get; set; }
    public float Offset { get; set; }
}
public interface ICutsceneable
{
    public IEnumerator ThisCutscene();
    public void ForceEndCutscene();
}
public interface IRoom
{
    public void OnRoomLoaded(); //called from the roomsLoader when created
    public void OnRoomUnloaded();
}
public interface IMultipleRoom
{
    public Vector2 ExitPos { get; }
    public Generic_OnTriggerEnterEvents combinedCollider { get; }
}
public interface IGestureAttack
{
    public Vector2 gestureDirection { get; set; }
}
public interface IRoomWithEnemies
{
    public Action OnAllEnemiesKilled { get; set; } //This is currently used for environemental hazards to stop, maybe music too
    public Action OnEnemiesSpawned { get; set; }
    public List<GameObject> CurrentlySpawnedEnemies { get; set; } //Remember to remove the enemies from this list whenever they die (if you destroy them)
}
#endregion