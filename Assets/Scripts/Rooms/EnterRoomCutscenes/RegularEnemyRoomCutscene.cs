using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnterTriggerCutscene;

public class RegularEnemyRoomCutscene : BaseCutsceneLogic
{
    [SerializeField] Transform CenterOfRoom;
    [SerializeField] RoomWithEnemiesLogic enemyRoomLogic;
    [SerializeField] bool skipFocusCamera;

    public override void playThisCutscene()
    {
        if (enemyRoomLogic.isRoomPermanentlyCompleted) { return; }

        if (skipFocusCamera) { currentCutscene = StartCoroutine(skippingCutscene()); return; }

        currentCutscene = StartCoroutine(RegularCutscene());
    }
    IEnumerator skippingCutscene()
    {
        yield return new WaitForSeconds(1);
        onCutsceneOver?.Invoke();
    }
    IEnumerator RegularCutscene()
    {
        
        //Find the references
        CameraZoomController zoomer = GameObject.Find(Tags.CMvcam1).GetComponent<CameraZoomController>();
        Player_FollowMouseWithFocus_V2 followMouse = GlobalPlayerReferences.Instance.references.followMouse;

        //Player_EventSystem playerEvents = GlobalPlayerReferences.Instance.references.events;
        //playerEvents.CallDisable();

        //Wait just in case for enemies to spawn
        yield return new WaitForSeconds(0.3f);

            //Look at center of room with zoom
            zoomer.AddZoomInfoAndUpdate(new CameraZoomController.ZoomInfo(6.5f, 3, "enterCutscene"));
            TargetGroupSingleton.Instance.AddTarget(CenterOfRoom, 20, 10);

            yield return new WaitForSeconds(0.9f);
        

        //Activate the Agroo of the enemies
        foreach (GameObject enemy in enemyRoomLogic.CurrentlySpawnedEnemies)
        {
            Enemy_References enemyRefs = enemy.GetComponent<Enemy_References>();
            enemyRefs.stateController.ForceAgroo();
        }
        
        followMouse.FocusNewEnemy(enemyRoomLogic.CurrentlySpawnedEnemies[0]);

        yield return new WaitForSeconds(0.5f);

            //Return to normal zoom
            zoomer.RemoveZoomInfoAndUpdate("enterCutscene");
            TargetGroupSingleton.Instance.RemoveTarget(CenterOfRoom);
        
        //playerEvents.CallEnable();

        onCutsceneOver?.Invoke();
        yield return null;
    }
}
