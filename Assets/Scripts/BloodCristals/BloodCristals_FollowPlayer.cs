using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodCristals_FollowPlayer : MonoBehaviour
{

    GameObject Player;
    Rigidbody2D bloodCristalRB;
    [SerializeField] float FollowVelocity;
    [SerializeField] float SpinningVelocity;
    [SerializeField] float MinPower = 20;
    [SerializeField] float MaxPower = 50;
    bool playerInRange;
    [SerializeField] Generic_OnTriggerEnterEvents playerProximityTrigger;
    [SerializeField] Generic_OnTriggerEnterEvents destroyerTrigger;
    private void OnEnable()
    {
        playerProximityTrigger.AddActivatorTag(Tags.Player);
        playerProximityTrigger.OnTriggerEntered += playerEntered;
        playerProximityTrigger.OnTriggerExited += playerExited;
        destroyerTrigger.AddActivatorTag(Tags.Player);
        destroyerTrigger.OnTriggerEntered += destroyItself;
    }
    private void OnDisable()
    {
        playerProximityTrigger.OnTriggerEntered -= playerEntered;
        playerProximityTrigger.OnTriggerExited -= playerExited;
        destroyerTrigger.OnTriggerEntered -= destroyItself;
    }
    void Start()
    {
        Player = GameObject.Find("MainCharacter");
        bloodCristalRB = GetComponent<Rigidbody2D>();
        RotateAndPush();
    }
    void playerEntered(Collider2D collision)
    { playerInRange = true; }
    void playerExited(Collider2D collision)
    { playerInRange = false; }
    void destroyItself(Collider2D collision)
    { Destroy(gameObject); }
    void FixedUpdate()
    {
        if (playerInRange)
        {
            transform.up = (Vector3.RotateTowards(transform.up, Player.transform.position - new Vector3(transform.position.x, transform.position.y), 10 * Time.deltaTime, 10));
            transform.Translate(new Vector2(SpinningVelocity * Time.deltaTime, FollowVelocity * Time.deltaTime));
        }
        if (Input.GetKeyDown(KeyCode.U)) { RotateAndPush(); }
        
    }
    
    void RotateAndPush()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        bloodCristalRB.AddForce(transform.up*Random.Range(MinPower,MaxPower));
    }
    
}
