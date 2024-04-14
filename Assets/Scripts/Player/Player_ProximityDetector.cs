using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ProximityDetector : MonoBehaviour
{
    [SerializeField] FloatVariable distanceToEnemy;
    [SerializeField] BoolVariable isDistanceToMouse;
    [SerializeField] TransformVariable ClosestEnemy;
    [SerializeField] Generic_OnTriggerEnterEvents proximityTrigger;

     Transform mouseTarget;

    List<Transform> InRangeEnemies = new List<Transform>();
    //Transform ClosestEnemy;
    Vector2 ownPosition, mousePosition;
    private void OnEnable()
    {
        proximityTrigger.AddActivatorTag(TagsCollection.Enemy);
        proximityTrigger.OnTriggerEntered += AddEnemy;
        proximityTrigger.OnTriggerExited += RemoveEnemy;
        mouseTarget = GameObject.Find(TagsCollection.MouseCameraTarget).transform;
    }
    private void Update()
    {
        ownPosition = transform.position;
        //If there are no enemies near, check distance with mouse
        if (InRangeEnemies.Count == 0)
        {
            mousePosition = mouseTarget.position;
            isDistanceToMouse.Value = true; 
            distanceToEnemy.Value = (mousePosition - ownPosition).magnitude;
            return; 
        }

        CheckClosest(); //Given the list of near enemies, check who is closests
        isDistanceToMouse.Value = false;

        distanceToEnemy.Value = (ClosestEnemy.Tf.position - transform.position).magnitude;
    }
    void AddEnemy(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo args)
    {
        InRangeEnemies.Add(args.Collision.gameObject.transform);
        args.Collision.gameObject.GetComponent<Generic_EventSystem>().OnDeath += EnemyDied;
    }
    void RemoveEnemy(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo args)
    {
        InRangeEnemies.Remove(args.Collision.gameObject.transform);
        args.Collision.gameObject.GetComponent<Generic_EventSystem>().OnDeath -= EnemyDied;
    }
    void EnemyDied(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        InRangeEnemies.Remove(args.DeadGameObject.transform);
    }
    void CheckClosest()
    {
        Transform currentClosest = null;
        float closestDistanse = 100;
        foreach (Transform t in InRangeEnemies)
        {
            float thisDistance = (t.position - transform.position).magnitude;
            if (thisDistance < closestDistanse)
            {
                closestDistanse = thisDistance;
                currentClosest = t;
            }
        }
        ClosestEnemy.Tf = currentClosest;
    }
}
