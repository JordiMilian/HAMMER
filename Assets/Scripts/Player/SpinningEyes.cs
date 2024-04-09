using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SpinningEyes : MonoBehaviour
{
    [SerializeField] Transform EyeBone;
    [SerializeField] Transform HeadBone;
    [SerializeField] Transform SpritesRoot;
    [SerializeField] RotationConstraint constraint;
    [SerializeField] Player_FollowMouse_withFocus followMouse;
    ConstraintSource constranitSource = new ConstraintSource();
    Camera mainCamera;
    bool isFocusingOnEnemy;
    Vector2 targetPos;
    private void Awake()
    {
        mainCamera = Camera.main;

        constranitSource.sourceTransform = HeadBone;
        constranitSource.weight = 1;
        constraint.AddSource(constranitSource);
    }

    private void Update()
    {
        Vector2 rootPos = transform.position;

        //If there is a focused enemy, look at enemy, else look at mouse
        if(followMouse.FocusedEnemy != null) { targetPos = followMouse.FocusedEnemy.transform.position; }
        else { targetPos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition); }

        //Find the direction to the target, whatever it is
        Vector2 directionTomouse = (targetPos-rootPos).normalized * simplifyFloat(SpritesRoot.localScale.x);

        //Set
        //EyeBone.right = directionTomouse;
        EyeBone.right = Vector3.RotateTowards(EyeBone.right, directionTomouse, 10 * Time.deltaTime, 10f);
    }
    int simplifyFloat(float f)
    {
        if (f >= 0) { return 1; }
        else return -1;
    }
}
