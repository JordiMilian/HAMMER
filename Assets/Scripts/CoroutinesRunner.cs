using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutinesRunner : MonoBehaviour
{
    public static CoroutinesRunner instance;
    Dictionary<string, Coroutine> runningCoroutines = new();


    private void Awake()
    {
       if(instance != null)
        {
            Destroy(gameObject);
        }
        else { instance = this; }
    }
    
    public void RunCoroutine(IEnumerator coroutine, string identifier = "")
    {
        if(identifier == "")
        {
            StartCoroutine(coroutine);
            return;
        }

        runningCoroutines.Add(identifier, StartCoroutine(coroutine));
    }
    public void EndCoroutine(string identifier)
    {
        CleanNullCoroutines();

        if(runningCoroutines.ContainsKey(identifier))
        {
            Coroutine coroutine = runningCoroutines[identifier];
            StopCoroutine(coroutine);
            runningCoroutines.Remove(identifier);
        }
    }

    void CleanNullCoroutines()
    {
        List<string> keysToRemove = new();
        foreach (KeyValuePair<string, Coroutine> cor in runningCoroutines)
        {
            if(cor.Value == null)
            {
                keysToRemove.Add(cor.Key);
            }
        }
        foreach (string key in keysToRemove)
        {
            runningCoroutines.Remove(key);
        }

    }
}
