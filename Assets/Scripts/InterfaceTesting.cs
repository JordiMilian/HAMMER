using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceTesting : MonoBehaviour
{
    public event Action onFinished;
    public void playCutscene()
    {
        onFinished?.Invoke();
    }
    public Action OnCutsceneOver()
    {
        return new Action(() => { Debug.Log("action"); });
    }
}
