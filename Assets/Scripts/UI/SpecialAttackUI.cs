using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackUI : MonoBehaviour
{
    [SerializeField] Transform Size01;
    [SerializeField] FloatVariable MaxSpecialAttack;
    [SerializeField] FloatVariable CurrentSpecialAttack;
    [SerializeField] Color FullColor, ChargingColor;
    [SerializeField] Transform TutorialsRoot, StarRoot;
    [SerializeField] SpriteRenderer BarSprite;
    [SerializeField] Animator animator;

    private void OnEnable()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        CurrentSpecialAttack.OnValueSet += updateBar;
        updateBar();
    }
    private void OnDisable()
    {
        CurrentSpecialAttack.OnValueSet -= updateBar;
    }
    void updateBar()
    {
        animator.SetTrigger("Update");
        float normalizedSpAt = Mathf.InverseLerp(0, MaxSpecialAttack.Value, CurrentSpecialAttack.Value);
        Size01.localScale = new Vector3(normalizedSpAt, 1, 1);
        if(CurrentSpecialAttack.Value >= MaxSpecialAttack.Value) { OnFullyCharged(); }
        else { OnNotFullyCharged() ; }
    }
    void OnFullyCharged()
    {
        BarSprite.color = FullColor;
        TutorialsRoot.gameObject.SetActive(true);
        StarRoot.gameObject.SetActive(true);
        animator.SetBool("Star", true);
    }
    void OnNotFullyCharged()
    {
        BarSprite.color = ChargingColor;
        TutorialsRoot.gameObject.SetActive(false);
        StarRoot.gameObject.SetActive(false);
        animator.SetBool("Star", false);
    }
}
