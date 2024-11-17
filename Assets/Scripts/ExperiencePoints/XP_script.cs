using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP_script : MonoBehaviour
{
    public int XpAmount;
    [SerializeField] Animator xpAnimator;
    [SerializeField] float minDistance, maxDistance, minTime, maxTime;
    [SerializeField] AnimationCurve spawnMovementCurve;

    public void onSpawn() //Called from XP_dropper
    {
        float averageValueOfCurve = UsefullMethods.GetAverageValueOfCurve(spawnMovementCurve,10);
        StartCoroutine( UsefullMethods.ApplyCurveMovementOverTime(
            GetComponent<Generic_CharacterMover>(),
            Random.Range(minDistance, maxDistance),
            Random.Range(minTime, maxTime),
            UsefullMethods.angle2Vector(Random.Range((float)0, (float)1) * Mathf.PI * 2),
            spawnMovementCurve,
            averageValueOfCurve
            ));
    }
    public void onPickedUp() //Called from player_experienceCollector
    {
        Destroy(gameObject);
    }
}
