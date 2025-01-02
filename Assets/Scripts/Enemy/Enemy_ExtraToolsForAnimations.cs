using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy_ExtraToolsForAnimations : MonoBehaviour
{
    [SerializeField] float setRotationSpeed;
    [SerializeField] Enemy_References enemyRefs;

    //THIS NEEDS TESTING PLSSS PLS PLSPLS que perea
    public void EV_StopMoving()
    {
        enemyRefs.moveToTarget.DoMove = false;
    }
    public void EV_ContinueMoving()
    {
        enemyRefs.moveToTarget.DoMove = true;
    }
    public void EV_StopLookingAtPlayer()
    {
        enemyRefs.moveToTarget.DoLook = false;
    }
    public void EV_ContinueLookingAtPlayer()
    {
        enemyRefs.moveToTarget.DoLook = true;
        Debug.Log("continue looking");
    }
    public void EV_SetWeaponPivotRotation(float rotation)
    {
        StartCoroutine(slowlyTurnTransform(
            enemyRefs.lookingPivotTf,
            rotation,
            setRotationSpeed
            ));
    }
    IEnumerator slowlyTurnTransform(Transform rotatedTf, float newRotation, float timeToRotate)
    {
        float timer = 0;
        float normalizedTime = 0;
        float startingRotation = rotatedTf.eulerAngles.z;

        while(timer < timeToRotate)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / timeToRotate;
            float calculatedRotation = Mathf.Lerp(startingRotation, newRotation, normalizedTime);

            rotatedTf.eulerAngles = new Vector3(
                rotatedTf.eulerAngles.x,
                rotatedTf.eulerAngles.y,
                calculatedRotation
                );

            yield return null;
        }
    }
}
