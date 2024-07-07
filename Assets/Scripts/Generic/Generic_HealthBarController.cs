using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_HealthBarController : MonoBehaviour
{
    //[SerializeField] FloatVariable Bar_Value;
    //[SerializeField] FloatVariable Bg_Value;

    [SerializeField] Generic_HealthSystem health;
    FloatReference currentHP;
    FloatReference MaxHP;

    [SerializeField] Transform Bar_Tf;
    [SerializeField] Transform Bg_Tf;

    [SerializeField] float HorizontalSizePerUnit = .5f;

    Vector3 Bar_BaseScale;
    Vector3 Bg_BaseScale;
    [SerializeField] float Bg_HorizontalOffset;

    [Header("Testing")]
    [SerializeField] float Bar_testValue;
    [SerializeField] float BG_testValue;
    [SerializeField] bool testTrigger;

    private void Update()
    {
        if(testTrigger)
        {
            currentHP.ChangeValue(Bar_testValue);
            MaxHP.ChangeValue(BG_testValue);

            testTrigger = false;
        }
    }
    private void OnEnable()
    {
        currentHP = health.CurrentHP;
        MaxHP = health.MaxHP;

        currentHP.OnValueChanged += UpdateBarSize;
        MaxHP.OnValueChanged += UpdateBgSize;

        Bar_BaseScale = Bar_Tf.localScale;
        Bg_BaseScale = Bg_Tf.localScale;

        UpdateBarSize();
        UpdateBgSize();
    }

    void UpdateBarSize()
    {
        Bar_Tf.localScale = new Vector3(currentHP.GetValue() * HorizontalSizePerUnit, Bar_BaseScale.y, Bar_BaseScale.z);
    }

    void UpdateBgSize()
    {
        Bg_Tf.localScale = new Vector3((MaxHP.GetValue() * HorizontalSizePerUnit) + Bg_HorizontalOffset, Bg_BaseScale.y, Bg_BaseScale.z);
    }


}
