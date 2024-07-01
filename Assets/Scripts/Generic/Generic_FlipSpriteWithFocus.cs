using System;
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
    public Action OnFlipped;
    Coroutine currentDelay;
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
                if (currentDelay != null) { StopCoroutine(currentDelay); }
                currentDelay = StartCoroutine(FlipCooldown());
                lookingDirection = 1; // 1 = right, -1 = left
            }
        }
        else if (focus.x < gameObject.transform.position.x)
        {
            if (FlipOnce == true && canFlip)
            {
                flipSprite(SpriteObject);

                if(currentDelay != null) { StopCoroutine(currentDelay); }
                currentDelay = StartCoroutine(FlipCooldown());
                lookingDirection = -1;
            }   
        }
    }
    void flipSprite(GameObject objecto)
    {
        objecto.transform.eulerAngles = new Vector3(objecto.transform.eulerAngles.x, objecto.transform.eulerAngles.y +180, objecto.transform.eulerAngles.z);
        FlipOnce = !FlipOnce;
        OnFlipped?.Invoke();
    }
    IEnumerator FlipCooldown()
    {
        canFlip = false;
        yield return new  WaitForSeconds(FlipDelay);
        canFlip = true;
    }
}
