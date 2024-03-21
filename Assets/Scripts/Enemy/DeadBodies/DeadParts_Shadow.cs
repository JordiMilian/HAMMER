using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadParts_Shadow : MonoBehaviour
{
    [SerializeField] Transform DeadPart_TF;
    Transform shadow_TF;
    [SerializeField] Vector2 MaxScale;
    [SerializeField] Vector2 MinScale;
    [SerializeField] Vector2 MinMaxDistance;
    private void Start()
    {
        shadow_TF = transform;
    }
    private void Update()
    {
        float distanceToShadow = (DeadPart_TF.position - shadow_TF.position).magnitude;
        float normalizedDistance = Mathf.InverseLerp(MinMaxDistance.x,MinMaxDistance.y, distanceToShadow);
        Vector2 relativeScale = Vector2.Lerp(MaxScale,MinScale, normalizedDistance);
        shadow_TF.localScale = relativeScale;
    }
}
