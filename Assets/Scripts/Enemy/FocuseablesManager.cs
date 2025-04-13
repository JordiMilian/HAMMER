using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocuseablesManager : MonoBehaviour
{
    public List<Focuseable> FocusaeblesList = new List<Focuseable>();

    #region Singleton Logic
    public static FocuseablesManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion
}
