using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_FlipSpriteWithFocus : MonoBehaviour
{
    [HideInInspector] public bool FlipOnce;
    public Vector2 FocusVector;
    public GameObject SpriteObject;
    public float FlipDelay = 0.2f;
    [HideInInspector] public bool canFlip = true;
    [HideInInspector] public int lookingDirection;
    
    
    public void FixedUpdate()
    {
        FlipSpriteWithFocus(FocusVector, SpriteObject);
    }
    public virtual void FlipSpriteWithFocus(Vector2 focus, GameObject spriteObject)
    {
        if (focus.x > gameObject.transform.position.x)
        {
            if (FlipOnce == false && canFlip)
            {
                flipSprite(SpriteObject);

                StartCoroutine(FlipCooldown());
                lookingDirection = 1; // 1 = right, -1 = left
            }
            

        }
        else if (focus.x < gameObject.transform.position.x)
        {
            if (FlipOnce == true && canFlip)
            {
                flipSprite(SpriteObject);

                StartCoroutine(FlipCooldown());
                lookingDirection = -1;
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
