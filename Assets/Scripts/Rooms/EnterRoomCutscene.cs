using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnterRoomCutscene : MonoBehaviour
{
    [SerializeField] Generic_OnTriggerEnterEvents enterRoomTrigger;
    [SerializeField] EnemyGenerator enemyGenerator;
    [SerializeField] Transform CenterOfRoom;
    bool isRoomEntered;

    private void OnEnable()
    {
        enterRoomTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        enterRoomTrigger.OnTriggerEntered += callEntered;
    }
    private void OnDisable()
    {
        enterRoomTrigger.OnTriggerEntered -= callEntered;
    }
    void callEntered(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo args)
    {
        if (enemyGenerator.isRoomPermanentlyCompleted) { return; }
        if(!enemyGenerator.reenteredRoom) { return; }
        StartCoroutine(EnterRoom());
    }

    IEnumerator EnterRoom()
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
        foreach (GameObject enemy in enemyGenerator.CurrentlySpawnedEnemies)
        {
            enemy.GetComponent<Enemy_EventSystem>().CallAgrooState?.Invoke();
        }

        yield return new WaitForSeconds(0.9f);

        //Return to normal zoom
        zoomer.RemoveZoomInfoAndUpdate("enterCutscene");
        TargetGroupSingleton.Instance.RemoveTarget(CenterOfRoom);

        enemyGenerator.reenteredRoom = false;
    }
}
