using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato_FollowPlayer : MonoBehaviour
{
    Transform Player;
    void Start()
    {
        Player = GameObject.Find("MainCharacter").transform;
    }

    
    void Update()
    {
        Vector3 PlayerPos = (Vector3)Player.position;
        transform.up = (Vector3.RotateTowards(transform.up, PlayerPos - new Vector3(transform.position.x, transform.position.y), 100 * Time.deltaTime, 10));
    }
}
