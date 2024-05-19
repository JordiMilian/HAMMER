using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneReflectionTest : MonoBehaviour
{
    [SerializeField] Transform RealBoneReference;

    private void Update()
    {
        transform.localPosition = RealBoneReference.localPosition;
        transform.localScale = RealBoneReference.localScale;
        transform.localRotation = RealBoneReference.localRotation;
    }
}
