using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackUI : MonoBehaviour
{
    [SerializeField] Transform Size01;
    [SerializeField] FloatVariable MaxSpecialAttack;
    [SerializeField] FloatVariable CurrentSpecialAttack;

    private void OnEnable()
    {
        CurrentSpecialAttack.OnValueSet += updateBar;
    }
    private void OnDisable()
    {
        CurrentSpecialAttack.OnValueSet -= updateBar;
    }
    void updateBar()
    {
        float normalizedSpAt = Mathf.InverseLerp(0, MaxSpecialAttack.Value, CurrentSpecialAttack.Value);
        Size01.localScale = new Vector3(normalizedSpAt, 1, 1);
    }
}
