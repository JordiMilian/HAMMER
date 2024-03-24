using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadPart_RebornController : MonoBehaviour
{
    [SerializeField] Animator DeadpartAnimator;
    private void OnEnable()
    {
        StartCoroutine(delayedTrigger());
    }
    IEnumerator delayedTrigger()
    {
        yield return new WaitForSeconds(4);
        DeadpartAnimator.SetTrigger("Reborn");
    }
}
