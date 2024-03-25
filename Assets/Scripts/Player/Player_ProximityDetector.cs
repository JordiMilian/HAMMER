using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ProximityDetector : MonoBehaviour
{
    [SerializeField] FloatReference distanceToEnemy;
    [SerializeField] FloatReference defaultDistance;
    [SerializeField] Generic_OnTriggerEnterEvents proximityTrigger;

    List<Transform> InRangeEnemies = new List<Transform>();
    Transform ClosestEnemy;
    private void OnEnable()
    {
        proximityTrigger.AddActivatorTag(TagsCollection.Enemy);
        proximityTrigger.OnTriggerEntered += AddEnemy;
        proximityTrigger.OnTriggerExited += RemoveEnemy;
    }
    private void Update()
    {
        if (InRangeEnemies.Count == 0)
        {
            distanceToEnemy.Value = defaultDistance.Value;
            return; 
        }
        CheckClosest();
        distanceToEnemy.Value = (ClosestEnemy.position - transform.position).magnitude;
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
        ClosestEnemy = currentClosest;
    }
}
