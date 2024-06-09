using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnterTriggerCutscene;

public class RegularEnemyRoomCutscene : BaseCutsceneLogic
{
    [SerializeField] Transform CenterOfRoom;
    [SerializeField] RoomWithEnemiesLogic enemyRoomLogic;

    public override void playThisCutscene()
    {
        if (enemyRoomLogic.isRoomPermanentlyCompleted) { return; }

        StartCoroutine(RegularCutscene());
    }
    IEnumerator RegularCutscene()
    {
        //Find the references
        CameraZoomController zoomer = GameObject.Find(TagsCollection.CMvcam1).GetComponent<CameraZoomController>();

        //Wait just in case for enemies to spawn
        yield return new WaitForSeconds(0.3f);

        //Look at center of room with zoom
        zoomer.AddZoomInfoAndUpdate(new CameraZoomController.ZoomInfo(6.5f, 3, "enterCutscene"));
        TargetGroupSingleton.Instance.AddTarget(CenterOfRoom, 20, 10);

        yield return new WaitForSeconds(0.9f);

        //Activate the Agroo of the enemies
        foreach (GameObject enemy in enemyRoomLogic.CurrentlySpawnedEnemies)
        {
            enemy.GetComponent<Enemy_EventSystem>().CallAgrooState?.Invoke();
        }

        yield return new WaitForSeconds(0.9f);

        //Return to normal zoom
        zoomer.RemoveZoomInfoAndUpdate("enterCutscene");
        TargetGroupSingleton.Instance.RemoveTarget(CenterOfRoom);

    }
}
