using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CurveToAddForce : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] Animator animator;
    AnimationClip currentClip;
    AnimationCurve currentCurve;
    AnimatorClipInfo[] clipInfo;
    
    private void Start()
    {
        string path = "m_LocalPosition.x";
        currentClip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        currentCurve = AnimationUtility.GetEditorCurve(currentClip, EditorCurveBinding.FloatCurve(path, typeof(Transform), path));
        currentCurve.Evaluate(0);
    }
    private void Update()
    {
        float timer = Time.realtimeSinceStartup;
        Debug.Log(currentCurve.Evaluate(timer));
    }

}
