using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BaseAction : MonoBehaviour
{
    public virtual void Action(UI_Button button)
    {
        Debug.LogWarning("button pressed with no Action");
    }
}
