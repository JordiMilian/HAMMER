using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_StaminaBarController : MonoBehaviour
{
    [SerializeField] FloatVariable currentStamina;
    [SerializeField] FloatVariable maxStamina;
    [SerializeField] Player_Stamina stamina;

    [SerializeField] Transform Bar_Tf;
    [SerializeField] Transform Bg_Tf;

    [SerializeField] float HorizontalSizePerUnit = .5f;

    Vector3 Bar_BaseScale;
    Vector3 Bg_BaseScale;
    [SerializeField] float Bg_HorizontalOffset;


    private void Update()
    {
        if(!stamina.isFilled)
        {
            UpdateBarSize();
        }
        
    }
    private void OnEnable()
    {
        maxStamina.OnValueSet += UpdateBgSize;

        Bar_BaseScale = Bar_Tf.localScale;
        Bg_BaseScale = Bg_Tf.localScale;

        UpdateBarSize();
        UpdateBgSize();
    }

    void UpdateBarSize()
    {
        Bar_Tf.localScale = new Vector3(currentStamina.GetValue() * HorizontalSizePerUnit, Bar_BaseScale.y, Bar_BaseScale.z);
    }

    void UpdateBgSize()
    {
        Bg_Tf.localScale = new Vector3((maxStamina.GetValue() * HorizontalSizePerUnit) + Bg_HorizontalOffset, Bg_BaseScale.y, Bg_BaseScale.z);
    }


}
