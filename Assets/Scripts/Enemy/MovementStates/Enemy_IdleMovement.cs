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

    bool IsEnabled = false;

    GameObject DestinationGO;
    Vector2 RoaminCenterVector;

    private void OnEnable()
    {
        IsEnabled = true;
        RoaminCenterVector = transform.position;
        DestinationGO = Instantiate(new GameObject(),transform.position,Quaternion.identity);
        destinationSetter.target = DestinationGO.transform;

        aiPath.maxSpeed = WalkingSpeed;
        DecideWalk();


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
            DecideWalk();
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

            EnemyAnimator.SetBool("Walking", true);
            spriteFliper.FocusVector = newDestination;
        }
        else  
        { 
            EnemyAnimator.SetBool("Walking", false);
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
            Gizmos.color = new Color(1, 1, 1, 0.3f);
            GizmoCenter = transform.position;
        }
        Gizmos.DrawWireSphere(GizmoCenter, RoamingRadios);
    }

}
