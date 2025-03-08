using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_HealthBarController : MonoBehaviour 
{

    IStats istats;
    [SerializeField] Component iStats_Holder;
    private void OnValidate()
    {
        if (iStats_Holder != null)
        {
            if (iStats_Holder.GetComponent<IStats>() == null)
            {
                iStats_Holder = null;
            }
            else { istats = iStats_Holder.GetComponent<IStats>(); }

        }
    }

    EntityStats currentStats;

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
            currentStats.CurrentHp = Bar_testValue;
            currentStats.MaxHp = BG_testValue;

            testTrigger = false;
        }
    }
    private void OnEnable()
    {
        currentStats = istats.GetCurrentStats();

        currentStats.OnCurrentHpChange += UpdateBarSize;
        currentStats.OnMaxHpChange += UpdateBgSize;

        Bar_BaseScale = Bar_Tf.localScale;
        Bg_BaseScale = Bg_Tf.localScale;

        UpdateBarSize(currentStats.CurrentHp);
        UpdateBgSize(currentStats.MaxHp);
    }
    private void OnDisable()
    {
        currentStats.OnCurrentHpChange -= UpdateBarSize;
        currentStats.OnMaxHpChange -= UpdateBgSize;
    }

    void UpdateBarSize(float newCurrentHp)
    {
        Bar_Tf.localScale = new Vector3(newCurrentHp * HorizontalSizePerUnit, Bar_BaseScale.y, Bar_BaseScale.z);
    }

    void UpdateBgSize(float newMaxHp)
    {
        Bg_Tf.localScale = new Vector3((newMaxHp * HorizontalSizePerUnit) + Bg_HorizontalOffset, Bg_BaseScale.y, Bg_BaseScale.z);
    }


}
