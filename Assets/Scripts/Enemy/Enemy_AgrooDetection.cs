using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AgrooDetection : MonoBehaviour
{
    Enemy_FollowPlayer _followPlayer;
    [SerializeField] Animator UIAnimator;
    private void Start()
    {
        _followPlayer = GetComponentInParent<Enemy_FollowPlayer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            if(!_followPlayer.IsAgroo)
            {
                UIAnimator.SetTrigger("AgrooAlert");
                _followPlayer.IsAgroo = true;
            }
            
        }
    }
}
