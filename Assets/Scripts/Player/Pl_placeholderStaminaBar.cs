using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pl_placeholderStaminaBar : MonoBehaviour
{

    [SerializeField] FloatVariable CurrentStamina;
    [SerializeField] FloatVariable MaxStamina;
    [SerializeField] Generic_EventSystem eventSystem;
    [SerializeField] Transform StamBarSize1;

    private void Update()
    {
        float newSize = Mathf.InverseLerp(0, MaxStamina.Value, CurrentStamina.Value);
        StamBarSize1.localScale = new Vector3(newSize, 1, 1);
    }

}
