using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_FlipSpriteWithFocus : MonoBehaviour
{
    bool FlipOnce;
    public Vector2 FocusVector;
    public GameObject SpriteObject;
    
    private void FixedUpdate()
    {
        FlipSpriteWithFocus(FocusVector, SpriteObject);
    }
    void FlipSpriteWithFocus(Vector2 focus, GameObject spriteObject)
    {
        if (focus.x > gameObject.transform.position.x)
        {
            if (FlipOnce == false) flipSprite(SpriteObject);

        }
        if (focus.x < gameObject.transform.position.x)
        {
            if (FlipOnce == true) flipSprite(SpriteObject);
        }
    }
    void flipSprite(GameObject objecto)
    {
        objecto.transform.localScale = new Vector2(objecto.transform.localScale.x * -1, objecto.transform.localScale.y);
        FlipOnce = !FlipOnce;
    }
}
