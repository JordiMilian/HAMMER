using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyState_Agroo : State
{
    [SerializeField] bool showDebug;
    public bool PlayerIsInAnyRange;


    [HideInInspector] public EnemyState_Attack currentAttack;
    EnemyState_Attack ForcedNextAttack;
    bool isNextAttackForced;



    List<EnemyState_Attack> List_EnemyAttacks = new List<EnemyState_Attack>();
    public override void OnEnable()
    {
        //Find the player and chase him. Set the moveToTarget and stuff
        if(List_EnemyAttacks.Count == 0)
        {
            List_EnemyAttacks = GetComponentsInChildren<EnemyState_Attack>().ToList();
        }

        foreach (EnemyState_Attack attack in List_EnemyAttacks)
        {
            attack.rangeDetector.OnPlayerEntered += attack.isInRange;
            attack.rangeDetector.OnPlayerExited += attack.isNotInRange;
            attack.rangeDetector.OnPlayerEntered += RunChecker;
            attack.rangeDetector.OnPlayerExited += RunChecker;
        }
    }
    public override void OnDisable()
    {
        //Should we disable the following scripts? Maybe no

        foreach (EnemyState_Attack attack in List_EnemyAttacks)
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

        bool CheckIfPlayerIsInAnyRange()
        {
            foreach (EnemyState_Attack attack in List_EnemyAttacks)
            {
                if (attack.isActive) return true;
            }
            return false;
        }
    }
    
    void FixedUpdate()
    {
        //bastant guarro aixo

        if (PlayerIsInAnyRange)
        {
            if (isNextAttackForced)
            {
                Debug.Log("Attack has been forced: " + ForcedNextAttack);
                isNextAttackForced = false;
                stateMachine.ChangeState(ForcedNextAttack);
                return;
            }

            //ResetAllTriggers(enemyRefs.animator); //Aixo crec que es pot borrar pero per si de cas nose
            EnemyState_Attack randomAvailableAttack = GetRandomAvailableAttack();
            if (randomAvailableAttack != null) { stateMachine.ChangeState(randomAvailableAttack); }
        }

    }

    public EnemyState_Attack GetRandomAvailableAttack()
    {
        //Make a list of all active attacks and not in Cooldown
        List<EnemyState_Attack> ActiveAttacks = new List<EnemyState_Attack>();
        foreach (EnemyState_Attack attack in List_EnemyAttacks)
        {
            if (attack.isActive && !attack.isInCooldown())
            {
                ActiveAttacks.Add(attack);
            }
        }
        if (ActiveAttacks.Count <= 0) { return null; } //If no attacks available return

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
            if ((randomFloat > probabilitatMinima) && (randomFloat < probabilitatMinima + ActiveAttacks[i].Probability))
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
        float AddAttacksProbability(List<EnemyState_Attack> attacks)
        {
            float Probabilities = 0;
            foreach (EnemyState_Attack attack in attacks) { Probabilities += attack.Probability; }
            return Probabilities;
        }

        void Debuguer(string message)
        {
            if(showDebug)
            {
                Debug.Log(message);
            }
                
        }
    }
}
