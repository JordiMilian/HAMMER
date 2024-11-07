using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMovementOvertime_test : MonoBehaviour
{
    [SerializeField] float testDistance, testTime;
    [SerializeField] AnimationCurve curve;
    [SerializeField] bool testTrigger, debugCurveTrigger;
    [SerializeField] int samplePoints;
    Generic_CharacterMover characterMover;
    private void Awake()
    {
        characterMover = GetComponent<Generic_CharacterMover>();
    }
    private void Update()
    {
        if(testTrigger)
        {
            applyMovement();
            testTrigger = false;
        }
        if(debugCurveTrigger)
        {
            checkAverage();
            debugCurveTrigger = false;
        }
    }
    void applyMovement()
    {
        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
            characterMover, testDistance, testTime, Vector2.right, curve, UsefullMethods.GetAverageValueOfCurve(curve,samplePoints)
            ));
    }
    void checkAverage()
    {
        Debug.Log(UsefullMethods.GetAverageValueOfCurve(curve, samplePoints));
    }
}