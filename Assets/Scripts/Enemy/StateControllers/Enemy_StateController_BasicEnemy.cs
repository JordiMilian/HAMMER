using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy_AttacksProviderV2;

#region INTERFACES
public interface IDamageReceiver
{
    public void OnReceiveDamage(object sender, Generic_EventSystem.ReceivedAttackInfo info);
}
public interface IParryReceiver
{
    public void OnGettingParried(Generic_EventSystem.GettingParriedInfo info);
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
public interface IAttacker
{
    public void PerformAttack(EnemyAttack enemyAttack);
}
#endregion

public class Enemy_StateController_BasicEnemy : MonoBehaviour, IDamageReceiver, IParryReceiver, IDeath, IStanceBreakable, IPlayerDetectable, IAttacker
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

    #region RECEIVE DAMAGE

    [SerializeField] AnimationCurve damagedMovementCurve;
    float damagedCurveAverage = .5f;
    bool calculatedAverage;
    public virtual void OnReceiveDamage(object sender, Generic_EventSystem.ReceivedAttackInfo info)
    {
        enemyRefs.animator.SetTrigger(Tags.PushBack);

        if (!calculatedAverage) { damagedCurveAverage = UsefullMethods.GetAverageValueOfCurve(damagedMovementCurve, 10); calculatedAverage = true; }

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
    #region PARRY
    public virtual void OnGettingParried(Generic_EventSystem.GettingParriedInfo info)
    {
        enemyRefs.animator.SetTrigger(Tags.HitShield);
        enemyRefs.showHideAttackCollider.EV_Enemy_HideAttackCollider();
        enemyRefs.moveToTarget.EV_SlowMovingSpeed();
        enemyRefs.moveToTarget.EV_SlowRotationSpeed();
    }
    #endregion
    #region DEATH
    public virtual void OnDeath(object sender, Generic_EventSystem.DeadCharacterInfo info)
    {
        StartCoroutine(UsefullMethods.destroyWithDelay(0.1f, enemyRefs.gameObject));
        TimeScaleEditor.Instance.SlowMotion(80, 1f); //This should be managed from the player or the room? not here som we can change the amount of slowmo depending on stuff
    }
    #endregion
    #region STANCE BROKEN
    public virtual void OnStanceBroken()
    {
        enemyRefs.animator.SetTrigger(Tags.StanceBroken);
        enemyRefs.moveToTarget.EV_SlowMovingSpeed();
        enemyRefs.moveToTarget.EV_SlowRotationSpeed();
    }
    #endregion
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
    #region ATTACKING

    [HideInInspector] public EnemyAttack currentAttack;
    bool isNextAttackForced;
    EnemyAttack ForcedNextAttack;
    private void FixedUpdate()
    {
        if (enemyRefs.attackProvider.PlayerIsInAnyRange && enemyRefs.animator.GetBool(Tags.InAgroo) && enemyRefs.attackProvider.isProviding)
        {
            if (isNextAttackForced)
            {
                Debug.Log("Attack has been forced: " + ForcedNextAttack.ShortDescription);
                PerformAttack(ForcedNextAttack);
                isNextAttackForced = false;
                return;
            }

            //ResetAllTriggers(enemyRefs.animator); //Aixo crec que es pot borrar pero per si de cas nose
            EnemyAttack randomAvailableAttack = enemyRefs.attackProvider.GetRandomAvailableAttack();
            if (randomAvailableAttack != null) { PerformAttack(randomAvailableAttack); }
        }
    }
    public virtual void PerformAttack(EnemyAttack selectedAttack)
    {
        foreach (Generic_DamageDealer dealer in enemyRefs.DamageDealersList)//Set stats to  damage dealers
        {
            SetDamageDealerStats(dealer, selectedAttack);
        }

        //Replace the StateMachines attack clip
        enemyRefs.reusableStateMachine.ReplaceaStatesClip(enemyRefs.reusableStateMachine.statesDictionary[Enemy_ReusableStateMachine.animationStates.BaseEnemy_Attacking], selectedAttack.animationClip);
        enemyRefs.animator.SetBool(Tags.InAgroo, false);
        enemyRefs.animator.SetTrigger(Tags.PerformAttack);


        //Cooldown if it has it
        if (selectedAttack.HasCooldown)
        {
            StartCoroutine(selectedAttack.Cooldown());
        }
        currentAttack = selectedAttack; //set current attack (this is used for the OVNI inverter currently

        //
        void SetDamageDealerStats(Generic_DamageDealer dealer, EnemyAttack selectedAttack)
        {
            dealer.Damage = selectedAttack.Damage * enemyRefs.currentEnemyStats.DamageMultiplicator;
            dealer.Knockback = selectedAttack.KnockBack;
            dealer.HitStop = selectedAttack.Damage * 0.1f; //Hitstop now depends on damage 
        }
    }
    public void ForceNextAttack(EnemyAttack forcedAttack)
    {
        ForcedNextAttack = forcedAttack;
        isNextAttackForced = true;
    }
    #endregion
}
