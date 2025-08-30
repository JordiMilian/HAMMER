using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(AttackTestingV2))]
public class AttackTesting_Editor : Editor
{
    

    AttackTestingV2 attackTesting;
    private void OnEnable()
    {
        attackTesting = (AttackTestingV2)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
     
        EditorGUILayout.HelpBox("This tool only works in PLAY MODE, make sure to start playing before using it", MessageType.Info);

        if (GUILayout.Button("START TESTING"))
        {
            if(attackTesting.Enemy == null)
            {
                EditorGUILayout.HelpBox("Set Enemy Reference First",MessageType.Warning);
                return;
            }
            ResetEnemyPos();
            attackTesting.enemyAttacks = attackTesting.Enemy.GetComponent<Enemy_References>().AttackStatesParent.GetComponentsInChildren<EnemyState_Attack>(true);

            Debug.Log($"Found {attackTesting.enemyAttacks.Length} attacks");

            attackTesting.trailsInfos.Clear();
            attackTesting.trails = attackTesting.Enemy.GetComponentsInChildren<TrailRenderer>();
            TrailInfo testingTrailInfo = GetTrailInfo(attackTesting.testTrailReference);
            for (int i = 0; i < attackTesting.trails.Length; i++)
            {
                attackTesting.trailsInfos.Add(GetTrailInfo(attackTesting.trails[i]));
                PasteTrailInfo(ref attackTesting.trails[i], testingTrailInfo);
            }
        }
        if (attackTesting.areReferenceMissing())
        {
            EditorGUILayout.HelpBox("Missing References", MessageType.Warning);
            return;
        }
        if (GUILayout.Button("END TESTING"))
        {
            for (int i = 0; i < attackTesting.trails.Length; i++)
            {
                PasteTrailInfo(ref attackTesting.trails[i], attackTesting.trailsInfos[i]);
            }
        }

        for(int i = 0; i < attackTesting.enemyAttacks.Length; i++)
        {
            EnemyState_Attack attack = attackTesting.enemyAttacks[i];

            GUILayout.BeginHorizontal();
            GUILayout.Label(attack.name, EditorStyles.boldLabel);
            GUILayout.Label(attack.rangeDetector.name, EditorStyles.whiteBoldLabel);
            if (GUILayout.Button("Perform"))
            {
                PerformAttack(i);
            }
           
            GUILayout.EndHorizontal();
        }
    }
    void ResetEnemyPos()
    {
        attackTesting.Enemy.transform.position = attackTesting.transform.position;
    }
    void PerformAttack(int i)
    {
        EnemyState_Attack attack = attackTesting.enemyAttacks[i];
        ResetEnemyPos();
        attackTesting.Enemy.GetComponent<Generic_StateMachine>().ChangeState(attack);
        attackTesting.performingAttackIndex = i;
    }
    void PasteTrailInfo(ref TrailRenderer trail, TrailInfo info)
    {
        trail.colorGradient = info.colorGradient;
        trail.widthCurve = info.widthCurve;
        trail.time = info.time;
    }
    TrailInfo GetTrailInfo(TrailRenderer trail)
    {
        TrailInfo info = new TrailInfo();
        info.colorGradient = trail.colorGradient;
        info.widthCurve = trail.widthCurve;
        info.time = trail.time;
        return info;
    }
}
