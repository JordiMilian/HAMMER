using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_CheckWalking : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float Threshold;
    [SerializeField] Animator animator;
    bool isWalking;
    Vector2 lastPosition;
    private void Start()
    {
        lastPosition = rb.position;
    }
    private void FixedUpdate()
    {
        float RBvelocity = (rb.position - lastPosition).magnitude;
        lastPosition = rb.position;

        Debug.Log(RBvelocity);
        if (RBvelocity <= Threshold && isWalking)
        {
            CheckNSetBool(TagsCollection.Walking, false);
            isWalking = false;
        }

        else if (RBvelocity > Threshold && !isWalking)
        {
            CheckNSetBool(TagsCollection.Walking, true);
            isWalking = true;
        }
    }
    void CheckNSetBool(string param, bool b)
    {
        if(HasParameter(param, animator))
        {
            animator.SetBool(param, b);
        }
        else { Debug.LogWarning(gameObject.name + " doesnt have the WALKING animator parameter, but dont worry its fine for now"); }
    }
     bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName) return true;
        }
        return false;
    }
}
