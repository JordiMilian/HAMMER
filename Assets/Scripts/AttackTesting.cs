using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTesting : MonoBehaviour
{
    public GameObject Enemy;
    public Enemy_AttacksProviderV2 AttackProviderV2;
    public BoxCollider2D AttackCollider;
    public Transform StartingPosition;
    public bool ResetPositionTrigger;
    public bool PerformAttack;
    
    [Range(0,10)]
    [SerializeField] int AttackToTest;

    void RestartPosition()
    {
        Enemy.transform.position = StartingPosition.position;
    }
    void TestAttack()
    {
        AttackProviderV2.PerformAttack(AttackProviderV2.Enemy_Attacks[AttackToTest]);
    }

    private void Update()
    {
        if(ResetPositionTrigger)
        {
            ResetPositionTrigger = false;
            RestartPosition();
        }
        if(PerformAttack)
        {
            PerformAttack = false;
            TestAttack();
        }

    }
}
