using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Enemy_AttacksProviderV2;

public class EnemyState_Agroo : EnemyState
{
    [SerializeField] bool showDebug;
    [SerializeField] Enemy_AgrooAlertIcon alertIconScript;
    public bool PlayerIsInAnyRange;
    public bool isPlayerDetected = false;

    [HideInInspector] public EnemyState_Attack currentAttack;
    EnemyState ForcedNextAttack;
    bool isNextAttackForced;
    [SerializeField] Transform TF_AttackStatets;


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
    private void OnDrawGizmosSelected()
    {
        foreach (EnemyState_Attack attack in List_EnemyAttacks)
        {
            if (attack.isActive) Gizmos.color = Color.blue;
            else Gizmos.color = Color.red;

            BoxCollider2D boxCollider = attack.rangeDetector.ownCollider;

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(boxCollider.transform.position, boxCollider.transform.rotation, boxCollider.transform.lossyScale);
            Gizmos.matrix = rotationMatrix;
            Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
        }
    }
}
