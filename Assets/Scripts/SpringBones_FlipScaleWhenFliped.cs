using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBones_FlipScaleWhenFliped : MonoBehaviour
{
    [SerializeField] Generic_FlipSpriteWithFocus spriteFlipper;
    private void OnEnable()
    {
        spriteFlipper.OnFlipped += flipItself;
    }
    private void OnDisable()
    {
        spriteFlipper.OnFlipped -= flipItself;
    }
    void flipItself() //DEPRECATED PLS DELETE THIS SCRIPT
    {
        transform.localScale = new Vector3 (transform.localScale.x * -1, 1, 1);
    }
}
