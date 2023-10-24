using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FollowMouse : MonoBehaviour
{

    public float FollowMouse_Speed = 0.08f;
    Player_Controller Player;
    void Start()
    {
        Player = GetComponentInParent<Player_Controller>();
    }
    void Update()
    {
        LookAtMouse();   
    }
    void LookAtMouse()
    {
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = (Vector3.RotateTowards(transform.up, mousePos - new Vector2(transform.position.x, transform.position.y), FollowMouse_Speed, 10f));
        //transform.up = (Vector3)(mousePos - new Vector2(transform.position.x, transform.position.y));
    }
}
