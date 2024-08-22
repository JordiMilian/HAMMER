using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipWeaponSpriteWithDirection : MonoBehaviour
{
    [SerializeField] Transform weapon_FollowPlayer;
    [SerializeField] List<Transform> weapon_spriteRoot = new List<Transform>();
    bool isFlipped;

    private void Update()
    {

        if (!isFlipped)
        {
            if (weapon_FollowPlayer.up.x < 0)
            {
                flip();
                isFlipped = true;
            }
        }
        else
        {
            if(weapon_FollowPlayer.up.x > 0)
            {
                unflip();
                isFlipped = false;
            }
        }
    }
    void flip()
    {
        foreach(Transform t in weapon_spriteRoot)
        {
            t.localEulerAngles = new Vector3(

                t.localEulerAngles.x,
                180,
                t.localEulerAngles.z
                );
        }
    }
    void unflip()
    {
        foreach (Transform t in weapon_spriteRoot)
        {
            t.localEulerAngles = new Vector3(

                t.localEulerAngles.x,
                0,
                t.localEulerAngles.z
                );
        }
    }
}
