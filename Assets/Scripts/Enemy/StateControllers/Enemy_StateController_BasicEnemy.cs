using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region INTERFACES
public interface IDamageReceiver
{
    public void OnReceiveDamage(object sender, Generic_EventSystem.ReceivedAttackInfo info);
}
public interface IParryReceiver
{
    public void OnGettingParried(int ownWeaponIndex);
}
public interface IDeath
{
    public void OnDeath(object sender, Generic_EventSystem.DeadCharacterInfo info);
}
public interface IPlayerDetectable
{
    public void OnPlayerDetected();
}
public interface IStanceBreakable
{
    public void OnStanceBroken();
}
#endregion

public class Enemy_StateController_BasicEnemy : MonoBehaviour, IDamageReceiver, IParryReceiver, IDeath, IStanceBreakable, IPlayerDetectable
{
    public Enemy_References enemyRefs;
    Enemy_EventSystem enemyEvents;
    private void OnEnable()
    {
        enemyEvents = enemyRefs.enemyEvents;
        enemyEvents.OnReceiveDamage += OnReceiveDamage;
        enemyEvents.OnGettingParried += OnGettingParried;
        enemyEvents.OnDeath += OnDeath;
        enemyEvents.OnStanceBroken += OnStanceBroken;

        enemyRefs.enemyEvents.OnEnterAgroo += enemyRefs.moveToTarget.EV_ReturnAllSpeed;

        substribeToAgrooTrigger();
    }

    #region Receive Damage

    [SerializeField] AnimationCurve damagedMovementCurve;
    float damagedCurveAverage = 0;
    public virtual void OnReceiveDamage(object sender, Generic_EventSystem.ReceivedAttackInfo info)
    {
        enemyRefs.animator.SetTrigger(Tags.PushBack);

        if (damagedCurveAverage !> 0) { damagedCurveAverage = UsefullMethods.GetAverageValueOfCurve(damagedMovementCurve, 10); }

        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
        enemyRefs.characterMover,
            info.KnockBack,
        0.25f,
            info.ConcreteDirection,
            damagedMovementCurve,
            damagedCurveAverage));

        enemyRefs.moveToTarget.EV_SlowRotationSpeed();
        enemyRefs.moveToTarget.EV_SlowMovingSpeed();
        StartCoroutine(WaitReceiveDamage());

        //
        IEnumerator WaitReceiveDamage()
        {
            yield return new WaitForSeconds(0.3f);
            enemyRefs.moveToTarget.EV_ReturnAllSpeed();
        }
    }
    #endregion
    public virtual void OnGettingParried(int i)
    {
        enemyRefs.animator.SetTrigger(Tags.HitShield);
        enemyRefs.showHideAttackCollider.EV_Enemy_HideAttackCollider();
        enemyRefs.moveToTarget.EV_SlowMovingSpeed();
        enemyRefs.moveToTarget.EV_SlowRotationSpeed();
    }

    public virtual void OnDeath(object sender, Generic_EventSystem.DeadCharacterInfo info)
    {
        StartCoroutine(UsefullMethods.destroyWithDelay(0.1f, enemyRefs.gameObject));
        TimeScaleEditor.Instance.SlowMotion(80, 1f); //This should be managed from the player or the room? not here som we can change the amount of slowmo depending on stuff
    }
    public virtual void OnStanceBroken()
    {
        enemyRefs.animator.SetTrigger(Tags.StanceBroken);
        enemyRefs.moveToTarget.EV_SlowMovingSpeed();
        enemyRefs.moveToTarget.EV_SlowRotationSpeed();
    }

    #region AGROO DETECTION
    [SerializeField] Generic_OnTriggerEnterEvents agrooTrigger;
    void substribeToAgrooTrigger()
    {
        if(agrooTrigger == null) { Debug.LogWarning("No agroo trigger here. Maybe it's not needed but whatever"); return; }
        agrooTrigger.AddActivatorTag(Tags.Player_SinglePointCollider);
        agrooTrigger.OnTriggerEntered += SomethingDetected;
    }
    public void ForceAgroo()
    {
        OnPlayerDetected();
    }
    void SomethingDetected(Collider2D collider)
    {
        if(collider.CompareTag(Tags.Player_SinglePointCollider))
        {
            OnPlayerDetected();
        }
    }
    public virtual void OnPlayerDetected()
    {
        enemyRefs.animator.SetTrigger(Tags.playerDetected);

        Transform PlayerTransform = GlobalPlayerReferences.Instance.playerTf;

        enemyRefs.moveToTarget.LookingTarget = PlayerTransform;
        enemyRefs.moveToTarget.DoLook = true;
        enemyRefs.moveToTarget.MovementTarget = PlayerTransform;
        enemyRefs.moveToTarget.DoMove = true;
        enemyRefs.attackProvider.isProviding = true;

        agrooTrigger.OnTriggerEntered -= SomethingDetected;

        enemyEvents.OnPlayerDetected?.Invoke();
    }
    #endregion
}
