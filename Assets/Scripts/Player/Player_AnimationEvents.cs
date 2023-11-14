using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AnimationEvents : MonoBehaviour
{
    Player_Controller playerController;
    Player_FollowMouse followMouse;
    Rigidbody2D rigidbody;
    [SerializeField] TrailRenderer WeaponTrail;
    [SerializeField] Collider2D DamageCollider;
    [SerializeField] Collider2D WeaponCollider;
    void Start()
    {
        playerController = GetComponent<Player_Controller>();
        followMouse = GetComponent<Player_FollowMouse>();   
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EV_ShowWeaponCollider() { WeaponCollider.enabled = true; }
    public void EV_HideWeaponCollider() { WeaponCollider.enabled = false; }
    public void EV_HideTrail() { WeaponTrail.enabled = false; }
    public void EV_ShowTrail() { WeaponTrail.enabled = true; }
    public void EV_HidePlayerCollider() { DamageCollider.enabled = false; }
    public void EV_ShowPlayerCollider() { DamageCollider.enabled = true; }

    //Refactor with Animation curve!!
    public void AddForce(float force)
    {
        rigidbody.AddForce(followMouse.gameObject.transform.up * force);
    }
    public void SlowDownSpeed(float slowspeed)
    {
        playerController.CurrentSpeed = slowspeed;
    }
    public void ReturnSpeed() { playerController.CurrentSpeed = playerController.BaseSpeed; }
}
