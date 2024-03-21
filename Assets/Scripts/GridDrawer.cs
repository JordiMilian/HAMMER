using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDrawer : MonoBehaviour
{
    LineRenderer lineRenderer;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        lineRenderer.positionCount = 1000;
    }
    private void Update()
    {
        Vector2 currentPoint = Vector2.zero;
    for (int i = 0; i < 100; i++)
    {
        lineRenderer.SetPosition(i, currentPoint);

        // Update the currentPoint based on the iteration
        if (i % 2 == 0)
        {
            // Move to the right
            currentPoint.x++;
        }
        else
        {
            // Move up or down
            currentPoint.y = (currentPoint.y == 0) ? -10 : 0;
        }
    }
    }
}
