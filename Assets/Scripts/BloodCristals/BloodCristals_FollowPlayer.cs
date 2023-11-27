using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodCristals_FollowPlayer : MonoBehaviour
{

    GameObject Player;
    Rigidbody2D rigidbody;
    [SerializeField] float FollowVelocity;
    [SerializeField] float SpinningVelocity;
    [SerializeField] float MinPower = 20;
    [SerializeField] float MaxPower = 50;
    public bool playerInRange;
    void Start()
    {
        Player = GameObject.Find("MainCharacter");
        rigidbody = GetComponent<Rigidbody2D>();
        RotateAndPush();
    }

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
        rigidbody.AddForce(transform.up*Random.Range(MinPower,MaxPower));
    }
    
}
