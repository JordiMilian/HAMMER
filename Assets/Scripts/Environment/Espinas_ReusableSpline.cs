using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Espinas_ReusableSpline : MonoBehaviour
{
    [SerializeField] SpriteShapeController MainShape;
    [SerializeField] SpriteShapeController thisShape;
    private void Awake()
    {
        CopySpriteShapeProperties(MainShape, thisShape);
    }
    void CopySpriteShapeProperties(SpriteShapeController source, SpriteShapeController target)
    {
        // Copy the SpriteShape
        target.spriteShape = source.spriteShape;

        // Copy the spline points
        var sourceSpline = source.spline;
        var targetSpline = target.spline;

        // Clear existing points
        targetSpline.Clear();

        // Add points from source spline
        for (int i = 0; i < sourceSpline.GetPointCount(); i++)
        {
            targetSpline.InsertPointAt(i, sourceSpline.GetPosition(i));
            targetSpline.SetTangentMode(i, sourceSpline.GetTangentMode(i));
            targetSpline.SetLeftTangent(i, sourceSpline.GetLeftTangent(i));
            targetSpline.SetRightTangent(i, sourceSpline.GetRightTangent(i));
        }

        // Set other properties if needed
        target.autoUpdateCollider = source.autoUpdateCollider;
        target.colliderOffset = source.colliderOffset;
        target.stretchTiling = source.stretchTiling;

        // If you have custom properties, copy them here as well
        // Example: target.customProperty = source.customProperty;

        // Update the target SpriteShape
        target.BakeCollider();
        target.RefreshSpriteShape();
    }
}
