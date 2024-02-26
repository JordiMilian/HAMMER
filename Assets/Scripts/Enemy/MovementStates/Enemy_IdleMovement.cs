using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_IdleMovement : MonoBehaviour
{
    // To-Do STATE MACHINE!!!!

    [SerializeField] float ChanceToChangeDestination;
    [SerializeField] float DelayBetweenChecks;
    [SerializeField] float WalkingSpeed;
    [SerializeField] float RoamingRadios = 2;
    [SerializeField] Animator EnemyAnimator;

    [SerializeField] AIDestinationSetter destinationSetter;
    [SerializeField] AIPath aiPath;
    [SerializeField] Generic_FlipSpriteWithFocus spriteFliper;
    [SerializeField] Enemy_MoveToTarget moveToTarget;

    bool IsEnabled = false;

    GameObject DestinationGO;
    Vector2 RoaminCenterVector;

    private void OnEnable()
    {
        IsEnabled = true;
        //Set a new Roaming center Vector at enemy position
        RoaminCenterVector = transform.position;
        //Set the destionation GO at the center too
        DestinationGO = Instantiate(new GameObject(),transform.position,Quaternion.identity);
        
        //destinationSetter.target = DestinationGO.transform;

        //aiPath.maxSpeed = WalkingSpeed;
        //DecideWalk();

        moveToTarget.Target = null;
        moveToTarget.DoMove = false;
    }
    private void OnDisable()
    {
        IsEnabled = false;
        CancelInvoke();
    }
    float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > DelayBetweenChecks)
        {
            //DecideWalk();
            timer = 0;
        }
    }
    void DecideWalk()
    {
        float randomFloat = Random.Range(0, 100);

        if (randomFloat <= ChanceToChangeDestination)
        {
            Vector2 newDestination = RoaminCenterVector + (Random.insideUnitCircle * RoamingRadios);

            DestinationGO.transform.position = newDestination;
            destinationSetter.target = DestinationGO.transform;

            if (HasParameter("Walking", EnemyAnimator)) { EnemyAnimator.SetBool("Walking", true); }
            
            spriteFliper.FocusVector = newDestination;
        }
        else  
        {
            if (HasParameter("Walking", EnemyAnimator)) EnemyAnimator.SetBool("Walking", false);
        }
    }
    private void OnDrawGizmos()
    {
        Vector2 GizmoCenter;
        if (IsEnabled)
        {
            Gizmos.color = Color.white;
            GizmoCenter = RoaminCenterVector;
        }
        else
        {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            GizmoCenter = transform.position;
        }
        Gizmos.DrawWireSphere(GizmoCenter, RoamingRadios);
    }
    //Method here just to check if the animator has a parameter because Unity doesnt have that option
    public static bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName) return true;
        }
        return false;
    }
}
