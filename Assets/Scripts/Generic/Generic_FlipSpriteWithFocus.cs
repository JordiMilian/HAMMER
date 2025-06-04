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
    [HideInInspector] public Vector2 lookingVector { get; private set; }
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
                lookingVector = Vector2.right;
            }
        }
        else if (focus.x < gameObject.transform.position.x)
        {
            if (FlipOnce == true && canFlip)
            {
                flipSprite(SpriteObject);

                if(currentDelay != null) { StopCoroutine(currentDelay); }
                currentDelay = StartCoroutine(FlipCooldown());
                lookingVector = Vector2.left;
            }   
        }
    }
    void flipSprite(GameObject objecto)
    {
        objecto.transform.localEulerAngles = new Vector3(objecto.transform.localEulerAngles.x, objecto.transform.localEulerAngles.y +180, objecto.transform.localEulerAngles.z);
        FlipOnce = !FlipOnce;
    }
    IEnumerator FlipCooldown()
    {
        canFlip = false;
        yield return new  WaitForSeconds(FlipDelay);
        canFlip = true;
    }
}
