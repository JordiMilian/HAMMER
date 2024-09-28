using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ProximityDetector : MonoBehaviour
{
    [SerializeField] FloatVariable distanceToEnemy;
    [SerializeField] BoolVariable isDistanceToMouse;
    [SerializeField] TransformVariable ClosestEnemy;
    [SerializeField] Generic_OnTriggerEnterEvents proximityTrigger;
    [SerializeField] Player_FollowMouse_alwaysFocus followMouse;

     Transform mouseTarget;

    List<Transform> InRangeEnemies = new List<Transform>();
    //Transform ClosestEnemy;
    Vector2 ownPosition, mousePosition;
    private void OnEnable()
    {
        proximityTrigger.AddActivatorTag(TagsCollection.Enemy);
        proximityTrigger.AddActivatorTag(TagsCollection.Enemy_notFocus);
        proximityTrigger.AddActivatorTag(TagsCollection.UpgradeContainer); //Aixo donara problemes en algun moment
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
            if(!InputDetector.Instance.isControllerDetected) //If keyboard, just check distance to mouse
            {
                mousePosition = mouseTarget.position;
                distanceToEnemy.Value = (mousePosition - ownPosition).magnitude;
            }
            else //if controller, make DOT between sword and movement and determine the distance with that
            {
                Vector2 currentSwordDirection = followMouse.SwordDirection.normalized;
                Vector2 movingDirection = InputDetector.Instance.MovementDirectionInput;

                float DotOfSwordNMovement = Vector2.Dot(currentSwordDirection, movingDirection);
                float normalizedDot = Mathf.InverseLerp(-1, 1, DotOfSwordNMovement);
                Player_ComboSystem_chargeless combos = GetComponent<Player_ComboSystem_chargeless>();
                float equivalentDistance = Mathf.Lerp(combos.minDistance, combos.maxDistance, normalizedDot);
                distanceToEnemy.Value = equivalentDistance;
            }
            
            isDistanceToMouse.Value = true;
            return; 
        }

        CheckClosest(); //Given the list of near enemies, check who is closests
        isDistanceToMouse.Value = false;

        distanceToEnemy.Value = (ClosestEnemy.Tf.position - transform.position).magnitude;
    }
    void AddEnemy(Collider2D collision)
    {
        InRangeEnemies.Add(collision.gameObject.transform);
        collision.gameObject.GetComponent<Generic_EventSystem>().OnDeath += EnemyDied;
    }
    void RemoveEnemy(Collider2D collision)
    {
        InRangeEnemies.Remove(collision.gameObject.transform);
        collision.gameObject.GetComponent<Generic_EventSystem>().OnDeath -= EnemyDied;
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
