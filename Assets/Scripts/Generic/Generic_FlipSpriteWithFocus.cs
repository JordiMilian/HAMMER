using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_FlipSpriteWithFocus : MonoBehaviour
{
    bool FlipOnce;
    public Vector2 FocusVector;
    public GameObject SpriteObject;
    float FlipDelay = 0.2f;
    bool canFlip = true;
    
    
    private void FixedUpdate()
    {
        FlipSpriteWithFocus(FocusVector, SpriteObject);
    }
    void FlipSpriteWithFocus(Vector2 focus, GameObject spriteObject)
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
        objecto.transform.localScale = new Vector2(objecto.transform.localScale.x * -1, objecto.transform.localScale.y);
        FlipOnce = !FlipOnce;
    }
    IEnumerator FlipCooldown()
    {
        canFlip = false;
        yield return new  WaitForSeconds(FlipDelay);
        canFlip = true;
        
    }
}
