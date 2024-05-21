using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.U2D;

public class Espina_PrefabsOnSplinePoints : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] SpriteShapeController espinaSplineController;

    private void Awake()
    {
        Transform espinaTf = espinaSplineController.transform;
        Vector2 espinaGOPos = espinaTf.position;
        Vector2 espinaLocalScale = new Vector2(espinaTf.localScale.x, espinaTf.localScale.y);

        for (int i = 0; i< espinaSplineController.spline.GetPointCount(); i++)
        {
            Vector2 pointPos = espinaSplineController.spline.GetPosition(i);
            Instantiate(obstaclePrefab, espinaGOPos + (pointPos*espinaLocalScale), Quaternion.identity);
        }
    }
}
