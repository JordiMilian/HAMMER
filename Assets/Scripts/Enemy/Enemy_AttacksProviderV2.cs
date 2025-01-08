using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Enemy_AttacksProviderV2 : MonoBehaviour
{

    [SerializeField] Enemy_References enemyRefs;
    [SerializeField] Enemy_ReusableStateMachine reusableStateMachine;

    [SerializeField] Generic_DamageDealer ExtraDamageDealer;
    public bool isProviding;
    public bool PlayerIsInAnyRange;
    [SerializeField] bool ShowDebug;
    

    public EnemyAttack[] Enemy_Attacks = new EnemyAttack[4];
    
    [HideInInspector] public EnemyAttack currentAttack;

    EnemyAttack ForcedNextAttack;
    bool isNextAttackForced;

    [Serializable]
    public class EnemyAttack
    {
        public string ShortDescription;
        public Enemy_AttackRangeDetector rangeDetector;
        [HideInInspector] public BoxCollider2D boxCollider;
        [HideInInspector] public bool isActive;
        public bool notOvniInvertable = false;
        
        [Header("Stats:")]
        public float Damage;
        public int Probability;
        public float KnockBack;
        [HideInInspector] public  float Hitstop;
        public AnimationClip animationClip;
        [Header("Cooldown")]
        public bool isInCooldown;
        public bool HasCooldown;
        public float CooldownTime;
  
        public  void isInRange() { isActive = true; }
        public void isNotInRange() { isActive = false; }
         
        public IEnumerator Cooldown()
        {
            isInCooldown = true;
            yield return new WaitForSeconds(animationClip.length + CooldownTime);
            isInCooldown = false;
        }
    }
    private void OnEnable()
    {
        //enemyRefs.enemyEvents.OnGettingParried += OnCancelAttackParried;
        //enemyRefs.enemyEvents.OnStanceBroken += OnCancelAttack;
        foreach (EnemyAttack attack in Enemy_Attacks)
        {
            attack.rangeDetector.OnPlayerEntered += attack.isInRange;
            attack.rangeDetector.OnPlayerExited += attack.isNotInRange;
            attack.rangeDetector.OnPlayerEntered += RunChecker;
            attack.rangeDetector.OnPlayerExited += RunChecker;
        }
    }
    private void OnDisable()
    {
        //enemyRefs.enemyEvents.OnGettingParried -= OnCancelAttackParried;
        //enemyRefs.enemyEvents.OnStanceBroken -= OnCancelAttack;
        foreach (EnemyAttack attack in Enemy_Attacks)
        {
            attack.rangeDetector.OnPlayerEntered -= attack.isInRange;
            attack.rangeDetector.OnPlayerExited -= attack.isNotInRange;
            attack.rangeDetector.OnPlayerEntered -= RunChecker;
            attack.rangeDetector.OnPlayerExited -= RunChecker;
        }
    }
    void RunChecker()
    {
        PlayerIsInAnyRange = CheckIfPlayerIsInAnyRange();
    }
    bool CheckIfPlayerIsInAnyRange()
    {
        foreach (EnemyAttack attack in Enemy_Attacks)
        {
            if (attack.isActive) return true;
        }
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        foreach (EnemyAttack attack in Enemy_Attacks)
        {
            if (attack.isActive) Gizmos.color = Color.blue;
            else Gizmos.color = Color.red;

            BoxCollider2D boxCollider = attack.rangeDetector.ownCollider;

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(boxCollider.transform.position, boxCollider.transform.rotation, boxCollider.transform.lossyScale);
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
        }
    }
    /*
    void FixedUpdate()
    {
        //bastant guarro aixo
        
        if (PlayerIsInAnyRange && enemyRefs.animator.GetBool(Tags.InAgroo) && isProviding) 
        {
            if(isNextAttackForced)
            {
                Debug.Log("Attack has been forced: " + ForcedNextAttack.ShortDescription);
                PerformAttack(ForcedNextAttack);
                isNextAttackForced = false;
                return;
            }

            //ResetAllTriggers(enemyRefs.animator); //Aixo crec que es pot borrar pero per si de cas nose
            EnemyAttack randomAvailableAttack = GetRandomAvailableAttack();
            if(randomAvailableAttack != null) { PerformAttack(randomAvailableAttack); }
        }
        
    }
    public void PerformAttack(EnemyAttack selectedAttack)
    {
        foreach(Generic_DamageDealer dealer in enemyRefs.DamageDealersList)//Set stats to  damage dealers
        {
            SetDamageDealerStats(dealer,selectedAttack);
        }

        //Replace the StateMachines attack clip
        reusableStateMachine.ReplaceaStatesClip(reusableStateMachine.statesDictionary[Enemy_ReusableStateMachine.animationStates.BaseEnemy_Attacking], selectedAttack.animationClip);
        enemyRefs.animator.SetBool(Tags.InAgroo,false);


        //Cooldown if it has it
        if(selectedAttack.HasCooldown)
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
    */
    
    public EnemyAttack GetRandomAvailableAttack()
    {
        //Make a list of all active attacks and not in Cooldown
        List<EnemyAttack> ActiveAttacks = new List<EnemyAttack>();
        foreach (EnemyAttack attack in Enemy_Attacks)
        {
            if (attack.isActive && !attack.isInCooldown)
            {
                ActiveAttacks.Add(attack);
            }
        }
        if(ActiveAttacks.Count <= 0) { return null; } //If no attacks available return

        //Add up all the probabilities and take a random number
        float ActiveAttacksProbability = AddAttacksProbability(ActiveAttacks);
        Debuguer("All Active attacks combined make: " + ActiveAttacksProbability);

        float randomFloat = UnityEngine.Random.Range(0, ActiveAttacksProbability);
        Debuguer("Random float is: " + randomFloat);

        //Check if the random number matches with the probabilities of the active attacks
        for (int i = 0; i < ActiveAttacks.Count; i++)
        {
            float probabilitatMinima = 0;
            for (int u = 0; u < i; u++)
            {
                probabilitatMinima += ActiveAttacks[u].Probability;
            }
            if ((randomFloat > probabilitatMinima) && ( randomFloat < probabilitatMinima + ActiveAttacks[i].Probability))
            {
                Debuguer(i + " succede chance");
                return ActiveAttacks[i];
            }
            else
            {
                Debuguer(i + " failed chance");
            }
        }
        return null;

        //
        float AddAttacksProbability(List<EnemyAttack> attacks)
        {
            float Probabilities = 0;
            foreach (EnemyAttack attack in attacks) { Probabilities += attack.Probability; }
            return Probabilities;
        }
    }
    
    public static void ResetAllTriggers(Animator anim)
    {
        foreach (var param in anim.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                anim.ResetTrigger(param.name);
            }
        }
    }
    
    void Debuguer(string text)
    {
        if (ShowDebug) { Debug.Log(text); }
    }
}
