using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTesting : MonoBehaviour
{
    [SerializeField] GameObject Enemy;
    [SerializeField] Enemy_AttacksProviderV2 AttackProviderV2;
    [SerializeField] string AttackName;
    BoxCollider2D AttackCollider;
    Transform StartingPosition;
    [SerializeField] bool ResetPositionTrigger;
    [SerializeField] bool PerformAttack;
    
    [Range(0,10)]
    [SerializeField] int AttackToTest;
    private void Awake()
    {
        Enemy.GetComponent<Generic_ShowHideAttackCollider>().isTesting = true;
    }
    void RestartPosition()
    {
        StartingPosition = transform;
        Enemy.transform.position = StartingPosition.position;
    }
    void TestAttack()
    {
        AttackProviderV2.PerformAttack(AttackProviderV2.Enemy_Attacks[AttackToTest]);
        AttackName = AttackProviderV2.Enemy_Attacks[AttackToTest].animationClip.name;
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (Enemy_AttacksProviderV2.EnemyAttack attack in AttackProviderV2.Enemy_Attacks)
        {
            DrawCollider(attack.rangeDetector.GetComponent<BoxCollider2D>());
        }
        AttackCollider = AttackProviderV2.Enemy_Attacks[AttackToTest].rangeDetector.GetComponent<BoxCollider2D>();
        Gizmos.color = Color.blue;
        DrawCollider(AttackCollider);
    }
    void DrawCollider(BoxCollider2D boxCollider)
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(boxCollider.transform.localPosition, boxCollider.transform.localRotation, boxCollider.transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        
        Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);
    }

}
