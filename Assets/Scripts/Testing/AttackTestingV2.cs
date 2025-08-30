using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct TrailInfo
{
    public Gradient colorGradient;
    public AnimationCurve widthCurve;
    public float time;
}
public class AttackTestingV2 : MonoBehaviour
{
    public GameObject Enemy;
    [HideInInspector] public EnemyState_Attack[] enemyAttacks;
    public TrailRenderer testTrailReference;
    [HideInInspector] public int performingAttackIndex = -1;


    [HideInInspector] public List<TrailInfo> trailsInfos = new();
    [HideInInspector] public TrailRenderer[] trails;


    private void OnDrawGizmosSelected()
    {
        if(areReferenceMissing()) { return; }
        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            Gizmos.color = Color.red;
            BoxCollider2D boxCollider = enemyAttacks[i].rangeDetector.ownCollider;
            DrawCollider(boxCollider);
        }
        if(performingAttackIndex >= 0)
        {
            Gizmos.color = Color.cyan;
            DrawCollider(enemyAttacks[performingAttackIndex].rangeDetector.ownCollider);
        }
    }

    void DrawCollider(BoxCollider2D boxCollider)
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(boxCollider.transform.localPosition, boxCollider.transform.localRotation, boxCollider.transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Transform WeaponPivot = Enemy.transform.Find("Weapons_FollowPlayer");
        Vector2 PivotOffset = new Vector2(WeaponPivot.localPosition.x, WeaponPivot.localPosition.y);
        Gizmos.DrawWireCube((Vector2)transform.position + boxCollider.offset + PivotOffset, boxCollider.size);
    }
    public bool areReferenceMissing()
    {
        if(Enemy == null) { return true; }
        foreach(EnemyState_Attack attack in enemyAttacks)
        {
            if (attack == null) { return true; }
        }
        return false;
    }
}
