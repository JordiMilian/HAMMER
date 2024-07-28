using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PinkSaw_OrientationSetter : MonoBehaviour
{
    [SerializeField] SpriteShapeController shapeController;
    [SerializeField] Transform sawRoot;
    [SerializeField] Transform colliderTf;
    [SerializeField] Transform maskTf;
    Vector2 pos01, pos02;

    private void Start()
    {
        setOritentation();
    }
    void setOritentation()
    {
       pos01 = shapeController.spline.GetPosition(0);
       pos02 = shapeController.spline.GetPosition(1);
        Vector2 directionOfGround = new Vector2(1, 0);
        Vector2 directionToPos01 = (pos01 - pos02).normalized;
        float relationshipBetweenPoints = Vector2.Dot(directionOfGround, directionToPos01);
        sawRoot.localScale = new Vector2(Mathf.Abs(relationshipBetweenPoints), 1);
        colliderTf.right = directionToPos01;
        maskTf.right = - directionToPos01;

        Vector2 splinePos = shapeController.transform.position;
        maskTf.position = pos01 + splinePos ;
    }
}
