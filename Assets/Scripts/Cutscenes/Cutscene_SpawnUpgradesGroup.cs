using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_SpawnUpgradesGroup : BaseCutsceneLogic
{
    [SerializeField] UpgradesGroup upgradesGroup;
    TargetGroupSingleton targetGroupSingleton;
    public override void playThisCutscene()
    {
        currentCutscene = StartCoroutine(spawnCutscene());   
    }
    IEnumerator spawnCutscene()
    {
        Player_EventSystem playerEvents = GlobalPlayerReferences.Instance.references.events;
        playerEvents.CallDisable();

        targetGroupSingleton = TargetGroupSingleton.Instance;
        
        targetGroupSingleton.AddTarget(upgradesGroup.transform, 10, 1);
        targetGroupSingleton.RemovePlayersTarget();

        yield return new WaitForSeconds(1);

        upgradesGroup.onSpawnNewContainers();

        yield return new WaitForSeconds(1);

        targetGroupSingleton.RemoveTarget(upgradesGroup.transform);
        targetGroupSingleton.ReturnPlayersTarget();

        playerEvents.CallEnable();

        onCutsceneOver?.Invoke();
    }
}
