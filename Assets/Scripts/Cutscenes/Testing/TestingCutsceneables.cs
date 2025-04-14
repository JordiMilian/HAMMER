using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingCutsceneables : MonoBehaviour, ICutsceneable
{
    [SerializeField] bool Trigger_AddCutsceneable, Trigger_ForceCutscene;
    [SerializeField] float waitingTime;
    [SerializeField] Transform referenceTransform;
    [SerializeField] float timer;
    private void Update()
    {
        if(Trigger_AddCutsceneable)
        {
            Trigger_AddCutsceneable = false;
            CutscenesManager.Instance.AddCutsceneable(this);
            Debug.Log("Added cutsceneable: " + this);
        }
        if(Trigger_ForceCutscene)
        {
            Trigger_ForceCutscene = false;
            CutscenesManager.Instance.ForceNextCutscene(this);
            Debug.Log("Forced cutscene: " + this);
        }
    }
    public void ForceEndCutscene()
    {
        Debug.Log("ForceEndCutscene at: " + timer);
        referenceTransform.position = transform.position;
    }

    public IEnumerator ThisCutscene()
    {
        timer = 0;
        while (timer < waitingTime)
        {
            timer += Time.deltaTime;
            referenceTransform.position = transform.position;
            Debug.Log("Referencing: "+ referenceTransform);
            yield return null;
        }
        Debug.Log("waited " + waitingTime);

    }
}
