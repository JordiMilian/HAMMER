using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PinkSaw_OrientationSetter_Projectile : MonoBehaviour
{
    [SerializeField] PinkSaw_Projectile projectile;

    [SerializeField] Transform sawRoot;
    [SerializeField] Transform colliderTf;
    [SerializeField] Transform maskTf;
    [SerializeField] float minimSize = 0.2f;
    Vector2 pos01, pos02;


    public void setOritentation() //la mascara sha de adaptar a la orientacio etc etc etc etc asdkjalksdj
    {
        pos01 = projectile.InitialPos;
        pos02 = projectile.FinalPos;
        Vector2 directionOfGround = new Vector2(1, 0);
        Vector2 directionToPos01 = (pos01 - pos02).normalized;
        float relationshipBetweenPoints = Vector2.Dot(directionOfGround, directionToPos01);
        float maskScale = UsefullMethods.simplifyScale(relationshipBetweenPoints) * -1;
        maskTf.localScale = new Vector3(1,maskScale, 1);

        float sawScale = Mathf.Abs(relationshipBetweenPoints);
        if(sawScale < minimSize) { sawScale = minimSize; }
        sawRoot.localScale = new Vector2(sawScale, 1);

        colliderTf.right = directionToPos01;
        maskTf.right = - directionToPos01;

        //Vector2 splinePos = shapeController.transform.position;
        //maskTf.position = pos01 + splinePos ;
    }
}
