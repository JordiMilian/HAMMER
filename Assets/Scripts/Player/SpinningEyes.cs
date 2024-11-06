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
    [SerializeField] Player_FollowMouseWithFocus_V2 followMouse;
    [SerializeField] Transform ConstrainedBone;
    [SerializeField] Transform FlippingRoot;
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
        if(followMouse.CurrentlyFocusedEnemy != null) { targetPos = followMouse.CurrentlyFocusedEnemy.transform.position; }
        else { targetPos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition); }

        //Find the direction to the target, whatever it is
        Vector2 directionTomouse = (targetPos - rootPos).normalized; //* simplifyFloat(SpritesRoot.localScale.x);

        //Set
        //EyeBone.right = directionTomouse;
        ConstrainedBone.localRotation = FlippingRoot.localRotation;
        EyeBone.right = Vector3.RotateTowards(EyeBone.right, directionTomouse, 10 * Time.deltaTime, 10f);

        //ConstrainedBone.eulerAngles = new Vector3(0, FlippingRoot.eulerAngles.y, ConstrainedBone.rotation.z);//Match the rotation of the flipping GO because because siu
    }
    int simplifyFloat(float f)
    {
        if (f >= 0) { return 1; }
        else return -1;
    }
}
