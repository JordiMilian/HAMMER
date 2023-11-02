using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttacksProvider : MonoBehaviour
{
    Enemy01 enemy01;

    public EnemyAttack[] Enemy_Attacks = new EnemyAttack[4];

    float OnLong_MaxRange;
    float OnMid_MaxRange;
    float OnShort_MaxRange;
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
    private void Start()
    {
        enemy01 = GetComponent<Enemy01>();
    }
    void Update()
    {
        if ((OnShortRange == true) || (OnLongRange == true) || (OnMidRange == true))
        {
            if (enemy01.Attacking == false)
            {
                PickAvailableAttacks();
                
            }
        }
    }
    void PickAvailableAttacks()
    {
        if (OnLongRange == true)
        {
            OnLong_MaxRange = 0;
            for (int i = 0; i < Enemy_Attacks.Length; i++)
            {
                OnLong_MaxRange += Enemy_Attacks[i].ProbabilityLong;
            }
            //Debug.Log("Max Long is: " + OnLong_MaxRange);
            float randomLongFloat = UnityEngine.Random.Range(0f, OnLong_MaxRange);
            //Debug.Log("Random Long is " + randomLongFloat);

            for (int i = 0; i < Enemy_Attacks.Length; i++)
            {
                minim = 0;
                for (int u = 0; u < i; u++)
                {
                    minim += Enemy_Attacks[u].ProbabilityLong;
                }
                if (randomLongFloat > minim && randomLongFloat <= minim + Enemy_Attacks[i].ProbabilityLong)
                {
                    StartCoroutine(enemy01.Attack(Enemy_Attacks[i]));
                    //Debug.Log(i + " succed Long chance");
                }
                else { //Debug.Log(i + " failed Long chance");
                       }
            }
        }
        if (OnMidRange == true)
        {
            OnMid_MaxRange = 0;
            for (int i = 0; i < Enemy_Attacks.Length; i++)
            {
                OnMid_MaxRange += Enemy_Attacks[i].ProbabilityMid;
            }
            //Debug.Log("Max Mid is: " + OnMid_MaxRange);
            float randomMidFloat = UnityEngine.Random.Range(0f, OnMid_MaxRange);
            //Debug.Log("Random Mid is " + randomMidFloat);

            for (int i = 0; i < Enemy_Attacks.Length; i++)
            {
                minim = 0;
                for (int u = 0; u < i; u++)
                {
                    minim += Enemy_Attacks[u].ProbabilityMid;
                }
                if (randomMidFloat > minim && randomMidFloat <= minim + Enemy_Attacks[i].ProbabilityMid)
                {
                    StartCoroutine(enemy01.Attack(Enemy_Attacks[i]));
                    //Debug.Log(i + " succed Mid chance");
                }
                else { //Debug.Log(i + " failed Mid chance");
                       }
            }
        }
        if (OnShortRange == true)
        {
            OnShort_MaxRange = 0;
            for (int i = 0; i < Enemy_Attacks.Length; i++)
            {
                OnShort_MaxRange += Enemy_Attacks[i].ProbabilityShort;
            }
            //Debug.Log("Max Short is: " + OnShort_MaxRange);
            float randomShortFloat = UnityEngine.Random.Range(0f, OnShort_MaxRange);
            //Debug.Log("Random Short is " + randomShortFloat);

            for (int i = 0; i < Enemy_Attacks.Length; i++)
            {
                minim = 0;
                for (int u = 0; u < i; u++)
                {
                    minim += Enemy_Attacks[u].ProbabilityShort;
                }
                if (randomShortFloat > minim && randomShortFloat <= minim + Enemy_Attacks[i].ProbabilityShort)
                {
                    StartCoroutine(enemy01.Attack(Enemy_Attacks[i]));
                    //Debug.Log(i + " succed Short chance");
                }
                else { //Debug.Log(i + " failed Short chance");
                       }
            }
        }
    }

}
