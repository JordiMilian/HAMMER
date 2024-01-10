using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SetSpriteConstrainAsCamera : MonoBehaviour
{
    [SerializeField] RotationConstraint SpritesConstraint;

    void Start()
    {
        ConstraintSource CameraConstrain = new ConstraintSource();
        CameraConstrain.sourceTransform = Camera.main.transform;
        CameraConstrain.weight = 1;
        SpritesConstraint = GetComponentInChildren<RotationConstraint>();
        SpritesConstraint.AddSource(CameraConstrain);
    }

    
}
