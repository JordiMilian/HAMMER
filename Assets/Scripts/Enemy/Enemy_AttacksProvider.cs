using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttacksProvider : MonoBehaviour
{
    [SerializeField] Generic_DamageDealer damageDealer;
    [SerializeField] Animator enemyAnimator;
    public bool isAttacking;
    [SerializeField] bool ShowDebug;

    public EnemyAttack[] Enemy_Attacks = new EnemyAttack[4];

    public enum Ranges
    {
        Short,Mid,Long
    }

    float minim;

    public bool OnShortRange = false;
    public bool OnMidRange = false;
    public bool OnLongRange = false;

    [Serializable]
    public class EnemyAttack
    {
        public float Damage;
        public float AnimationTime;
        public float KnockBack;
        public float Hitstop;
        public string TriggerName;
        public int ProbabilityShort;
        public int ProbabilityMid;
        public int ProbabilityLong;
    }
    void Update()
    {
        if ((OnShortRange == true) || (OnLongRange == true) || (OnMidRange == true))
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

        damageDealer.Damage = selectedAttack.Damage;
        damageDealer.Knockback = selectedAttack.KnockBack;
        damageDealer.HitStop = selectedAttack.Hitstop;
        enemyAnimator.SetTrigger(selectedAttack.TriggerName);

        StartCoroutine(AttackCooldown(selectedAttack));

    }
    IEnumerator AttackCooldown(EnemyAttack selectedAttack)
    {
        yield return new WaitForSeconds(selectedAttack.AnimationTime);
        ResetAllTriggers();
        isAttacking = false;
    }
    void PickAvailableAttacks()
    {
        if (OnLongRange == true)
        {
            float OnLong_MaxRange = AddAttacksProbability(Ranges.Long);
            Debuguer("Max Long is: " + OnLong_MaxRange);

            float randomLongFloat = UnityEngine.Random.Range(0f, OnLong_MaxRange);
            Debuguer("Random Long is " + randomLongFloat);

            for (int i = 0; i < Enemy_Attacks.Length; i++)
            {
                minim = 0;
                for (int u = 0; u < i; u++)
                {
                    minim += Enemy_Attacks[u].ProbabilityLong;
                }
                if (randomLongFloat > minim && randomLongFloat <= minim + Enemy_Attacks[i].ProbabilityLong)
                {
                    PerformAttack(Enemy_Attacks[i]);
                    Debuguer(i + " succed Long chance");
                }
                else {
                    Debuguer(i + " failed Long chance");
                       }
            }
        }
        if (OnMidRange == true)
        {
            float OnMid_MaxRange = AddAttacksProbability(Ranges.Mid);
            Debuguer("Max Mid is: " + OnMid_MaxRange);

            float randomMidFloat = UnityEngine.Random.Range(0f, OnMid_MaxRange);
            Debuguer("Random Mid is " + randomMidFloat);

            for (int i = 0; i < Enemy_Attacks.Length; i++)
            {
                minim = 0;
                for (int u = 0; u < i; u++)
                {
                    minim += Enemy_Attacks[u].ProbabilityMid;
                }
                if (randomMidFloat > minim && randomMidFloat <= minim + Enemy_Attacks[i].ProbabilityMid)
                {
                    PerformAttack(Enemy_Attacks[i]);
                    Debuguer(i + " succed Mid chance");
                }
                else {
                    Debuguer(i + " failed Mid chance");
                       }
            }
        }
        if (OnShortRange == true)
        {
            float OnShort_MaxRange = AddAttacksProbability(Ranges.Short);
            Debuguer("Max Short is: " + OnShort_MaxRange);

            float randomShortFloat = UnityEngine.Random.Range(0f, OnShort_MaxRange);
            Debuguer("Random Short is " + randomShortFloat);

            for (int i = 0; i < Enemy_Attacks.Length; i++)
            {
                minim = 0;
                for (int u = 0; u < i; u++)
                {
                    minim += Enemy_Attacks[u].ProbabilityShort;
                }
                if (randomShortFloat > minim && randomShortFloat <= minim + Enemy_Attacks[i].ProbabilityShort)
                {
                    PerformAttack(Enemy_Attacks[i]);
                    Debuguer(i + " succed Short chance");
                }
                else {
                    Debuguer(i + " failed Short chance");
                       }
            }
        }
    }
    float AddAttacksProbability(Ranges range)
    {
        float Adder = 0;
        switch (range)
        {
            case Ranges.Long:
                
                for (int i = 0; i < Enemy_Attacks.Length; i++)
                {
                    Adder += Enemy_Attacks[i].ProbabilityLong;
                }
                break;

            case Ranges.Mid:
                for (int i = 0; i < Enemy_Attacks.Length; i++)
                {
                    Adder += Enemy_Attacks[i].ProbabilityMid;
                }
                break;

            case Ranges.Short:
                for (int i = 0; i < Enemy_Attacks.Length; i++)
                {
                    Adder += Enemy_Attacks[i].ProbabilityShort;
                }
                break;
        }
        return Adder;
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
    }
    public void EV_Enemy_HideAttackCollider()
    {
        damageDealer.GetComponent<Collider2D>().enabled = false;
    }
    void Debuguer(string text)
    {
        if (ShowDebug) { Debug.Log(text); }
    }
}
