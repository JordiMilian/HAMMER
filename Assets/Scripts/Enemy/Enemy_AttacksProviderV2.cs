using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_AttacksProviderV2 : MonoBehaviour
{
    [SerializeField] Generic_DamageDealer damageDealer;
    [SerializeField] Animator enemyAnimator;
    public bool isAttacking;
    public bool PlayerIsInAnyRange;
    [SerializeField] bool ShowDebug;
    [SerializeField] Generic_Stats stats;

    public EnemyAttack[] Enemy_Attacks = new EnemyAttack[4];
    [SerializeField] TrailRenderer trailrendered;

    [Serializable]
    public class EnemyAttack
    {
        public string ShortDescription;
        public Enemy_AttackRangeDetector rangeDetector;
        public bool isActive;
        
        [Header("Stats:")]
        public float Damage;
        public int Probability;
        public float KnockBack;
        public float Hitstop;
        public string TriggerName;
        public AnimationClip animationClip;
        [Header("Cooldown")]
        public bool isInCooldown;
        public int PerformancesDone;
        public int PerformancesBeforeCooldown;
        public float CooldownTime;


        public  void isInRange(object sender, EventArgs args) { isActive = true; }
        public void isNotInRange(object sender, EventArgs args) { isActive = false; }
         
        public IEnumerator Cooldown()
        {
            isInCooldown = true;
            yield return new WaitForSeconds(animationClip.length + CooldownTime);
            isInCooldown = false;
        }
    }
    private void OnEnable()
    {
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
        foreach (EnemyAttack attack in Enemy_Attacks)
        {
            attack.rangeDetector.OnPlayerEntered -= attack.isInRange;
            attack.rangeDetector.OnPlayerExited -= attack.isNotInRange;
            attack.rangeDetector.OnPlayerEntered -= RunChecker;
            attack.rangeDetector.OnPlayerExited -= RunChecker;
        }
    }
    public void RunChecker(object sender, EventArgs args)
    {
        PlayerIsInAnyRange = CheckIfPlayerIsInAnyRange();
    }
    public bool CheckIfPlayerIsInAnyRange()
    {
        foreach (EnemyAttack attack in Enemy_Attacks)
        {
            if (attack.isActive == true) return (true);
        }
        return (false);
    }
    private void OnDrawGizmos()
    {
        foreach (EnemyAttack attack in Enemy_Attacks)
        {
            if (attack.isActive) Gizmos.color = Color.blue;
            else Gizmos.color = Color.red;

            BoxCollider2D boxCollider = attack.rangeDetector.collider;

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(boxCollider.transform.position, boxCollider.transform.rotation, boxCollider.transform.lossyScale);
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
        }
    }
    void Update()
    {
        if (PlayerIsInAnyRange)
        {
            if (!isAttacking)
            {
                PickAvailableAttacks();
            }
        }
    }
    void PerformAttack(EnemyAttack selectedAttack)
    {
        isAttacking = true;

        damageDealer.Damage = selectedAttack.Damage * stats.DamageMultiplier;
        damageDealer.Knockback = selectedAttack.KnockBack;
        damageDealer.HitStop = selectedAttack.Hitstop;
        enemyAnimator.SetTrigger(selectedAttack.TriggerName);

        StartCoroutine(WaitAnimationTime(selectedAttack));

        selectedAttack.PerformancesDone++;
        if(selectedAttack.PerformancesDone == selectedAttack.PerformancesBeforeCooldown)
        {
            selectedAttack.PerformancesDone = 0;
            StartCoroutine(selectedAttack.Cooldown());
        }

    }
    IEnumerator WaitAnimationTime(EnemyAttack selectedAttack)
    {
        yield return new WaitForSeconds(selectedAttack.animationClip.length);
        ResetAllTriggers();
        isAttacking = false;
    }
    void PickAvailableAttacks()
    {
        //Make a list of all active attacks
        List<EnemyAttack> ActiveAttacks = new List<EnemyAttack>();
        foreach (EnemyAttack attack in Enemy_Attacks)
        {
            if (attack.isActive && !attack.isInCooldown)
            {
                ActiveAttacks.Add(attack);
            }
        }
        //Add up all the probabilities and take a random number
        float ActiveAttacksProbability = AddAttacksProbability(ActiveAttacks);
        Debuguer("All Active attacks combined make: " + ActiveAttacksProbability);

        float randomFloat = UnityEngine.Random.Range(0, AddAttacksProbability(ActiveAttacks));
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
                PerformAttack(ActiveAttacks[i]);
                Debuguer(i + " succede chance");
            }
            else
            {
                Debuguer(i + " failed chance");
            }
        }

    }
    float AddAttacksProbability(List<EnemyAttack> attacks)
    {
        float Probabilities = 0;
        foreach (EnemyAttack attack in attacks) { Probabilities += attack.Probability; }
        return Probabilities;
    }
    private void ResetAllTriggers()
    {
        foreach (var param in enemyAnimator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                enemyAnimator.ResetTrigger(param.name);
            }
        }
    }
    public void EV_Enemy_ShowAttackCollider()
    {
        damageDealer.GetComponent<Collider2D>().enabled = true;
        if(trailrendered != null) trailrendered.emitting = true;

    }
    public void EV_Enemy_HideAttackCollider()
    {
        damageDealer.GetComponent<Collider2D>().enabled = false;
        if (trailrendered != null) trailrendered.emitting = false;
    }
    void Debuguer(string text)
    {
        if (ShowDebug) { Debug.Log(text); }
    }
}
