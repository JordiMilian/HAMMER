using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_SpawnUpgrades : BaseCutsceneLogic
{

    public override void playThisCutscene()
    {
        currentCutscene = StartCoroutine(spawnCutscene());   
    }
    IEnumerator spawnCutscene()
    {
        yield return null;

        onCutsceneOver?.Invoke();
    }
}
