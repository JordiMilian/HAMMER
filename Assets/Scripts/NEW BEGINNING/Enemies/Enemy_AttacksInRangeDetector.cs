using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy_AttacksInRangeDetector : MonoBehaviour
{
    //This script MUST be attached to the GameObject holding all the Enemy Attacks

    List<EnemyState_Attack> List_EnemyAttacks = new List<EnemyState_Attack>();
    public bool PlayerIsInAnyRange;
    private void OnEnable()
    {
        List_EnemyAttacks = transform.GetComponentsInChildren<EnemyState_Attack>(true).ToList();

        foreach (EnemyState_Attack attack in List_EnemyAttacks)
        {
            attack.rangeDetector.OnPlayerEntered += attack.isInRange;
            attack.rangeDetector.OnPlayerExited += attack.isNotInRange;
            attack.rangeDetector.OnPlayerEntered += RunChecker;
            attack.rangeDetector.OnPlayerExited += RunChecker;
        }
    }
    private void OnDisable()
    {
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

        //
        bool CheckIfPlayerIsInAnyRange()
        {
            foreach (EnemyState_Attack attack in List_EnemyAttacks)
            {
                if (attack.isActive) return true;
            }
            return false;
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
