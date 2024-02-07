using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_FlipSpriteWithFocus_Animated : Generic_FlipSpriteWithFocus
{
    
    [SerializeField] Animator animator;


    public void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void FlipSpriteWithFocus(Vector2 focus, GameObject spriteObject)
    {
        if (focus.x > gameObject.transform.position.x)
        {
            if (FlipOnce == false && canFlip)
            {
                flipSprite(SpriteObject);
                StartCoroutine(FlipCooldown());
            }

        }
        if (focus.x < gameObject.transform.position.x)
        {
            if (FlipOnce == true && canFlip)
            {
                flipSprite(SpriteObject);
                StartCoroutine(FlipCooldown());
            }
        }
    }
    void flipSprite(GameObject objecto)
    {
        animator.SetTrigger("FlipsSprites");
        FlipOnce = !FlipOnce;
    }
    public void EV_flipSprite()
    {
        SpriteObject.transform.localScale = new Vector2(SpriteObject.transform.localScale.x * -1, SpriteObject.transform.localScale.y);
    }
    IEnumerator FlipCooldown()
    {
        canFlip = false;
        yield return new WaitForSeconds(FlipDelay);
        canFlip = true;

    }
}
