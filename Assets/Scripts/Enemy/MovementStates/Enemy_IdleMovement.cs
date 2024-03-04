using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_IdleMovement : MonoBehaviour
{
    [SerializeField] float DelayBetweenChecks;
    [SerializeField] float WalkingSpeed;
    [SerializeField] float RoamingRadios = 2;

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
        DestinationGO.name = ("Destination " + gameObject.name);
        
        DecideWalk();

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
            DecideWalk();
            timer = 0;
        }
    }
    void DecideWalk()
    {
        float randomFloat = Random.Range(0, 100);

        //50% chance to change direction 
        if (randomFloat <= 50)
        {
            Vector2 newDestination = RoaminCenterVector + (Random.insideUnitCircle * RoamingRadios);

            DestinationGO.transform.position = newDestination;
            moveToTarget.Target = DestinationGO.transform;

            spriteFliper.FocusVector = newDestination;
        }
    }
    private void OnDrawGizmosSelected()
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

}
