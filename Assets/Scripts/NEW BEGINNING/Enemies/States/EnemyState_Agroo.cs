using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyState_Agroo : EnemyState
{
    [SerializeField] bool showDebug;
    [SerializeField] Enemy_AgrooAlertIcon alertIconScript;
    public bool isPlayerDetected = false;
    [SerializeField] Enemy_AttacksInRangeDetector attacksInRangeDetector;

    [HideInInspector] public EnemyState_Attack currentAttack;
    EnemyState ForcedNextAttack;
    bool isNextAttackForced;
    Transform TF_AttackStatets;


    public void ForceNextAttack(EnemyState ForcedState)
    {
        ForcedNextAttack = ForcedState;
        isNextAttackForced = true;
    }
    List<EnemyState_Attack> List_EnemyAttacks = new List<EnemyState_Attack>();
    public override void OnEnable()
    {
        base.OnEnable();
        if (List_EnemyAttacks.Count == 0)
        {
            TF_AttackStatets = attacksInRangeDetector.transform;
            List_EnemyAttacks = TF_AttackStatets.GetComponentsInChildren<EnemyState_Attack>(true).ToList();
            
        }

        if (!isPlayerDetected)
        {
            alertIconScript.playAlertIcon();
            isPlayerDetected = true;
        }

        EnemyRefs.moveToTarget.SetTargets(GlobalPlayerReferences.Instance.playerTf);

        EnemyRefs.moveToTarget.SetMovementSpeed(MovementSpeeds.Regular);
        EnemyRefs.moveToTarget.SetRotatinSpeed(MovementSpeeds.Regular);

        EnemyRefs.moveToTarget.DoLook = true;
        EnemyRefs.moveToTarget.DoMove = true;

        EnemyRefs.animator.CrossFade(AnimatorStateName, 0.1f);
    }


    
    void FixedUpdate()
    {
        //bastant guarro aixo
        
        if (attacksInRangeDetector.PlayerIsInAnyRange)
        {
            if (isNextAttackForced)
            {
                Debug.Log("Attack has been forced: " + ForcedNextAttack);
                isNextAttackForced = false;
                stateMachine.ChangeState(ForcedNextAttack);
                return;
            }

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
                Debuguer(ActiveAttacks[i].name + " succede chance");
                return ActiveAttacks[i];
            }
            else
            {
                Debuguer(ActiveAttacks[i].name + " failed chance");
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
