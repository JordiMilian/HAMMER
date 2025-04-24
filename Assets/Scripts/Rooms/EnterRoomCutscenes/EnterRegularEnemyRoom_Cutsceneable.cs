using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnterTriggerCutscene;

public class EnterRegularEnemyRoom_Cutsceneable : MonoBehaviour, ICutsceneable
{
    [SerializeField] Transform CenterOfRoom;
    [SerializeField] RoomWithEnemiesLogic enemyRoomLogic;
    [SerializeField] bool skipFocusCamera;

    public IEnumerator ThisCutscene()
    {
        //Find the references
        CameraZoomController zoomer = GameObject.Find(Tags.CMvcam1).GetComponent<CameraZoomController>();
        Player_SwordRotationController swordRotation = GlobalPlayerReferences.Instance.references.swordRotation;
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;

        //Disable player 
        playerRefs.stateMachine.ForceChangeState(playerRefs.DisabledState);


        //Wait just in case for enemies to spawn
        yield return new WaitForSeconds(0.3f);

        //Look at center of room with zoom
        zoomer.AddZoomInfoAndUpdate(new CameraZoomController.ZoomInfo(6.5f, 3, "enterCutscene"));
        TargetGroupSingleton.Instance.AddTarget(CenterOfRoom, 20, 10);

        yield return new WaitForSeconds(0.9f);


        //Activate the Agroo of the enemies
        foreach (GameObject enemy in enemyRoomLogic.CurrentlySpawnedEnemies)
        {
            Generic_StateMachine stateMachine = enemy.GetComponent<Generic_StateMachine>();
            stateMachine.ChangeState(enemy.GetComponent<Enemy_References>().AgrooState);
        }

        swordRotation.FocusNewEnemy(enemyRoomLogic.CurrentlySpawnedEnemies[0].GetComponent<Enemy_References>().Focuseable);

        yield return new WaitForSeconds(0.5f);

        //Return to normal zoom
        zoomer.RemoveZoomInfoAndUpdate("enterCutscene");
        TargetGroupSingleton.Instance.RemoveTarget(CenterOfRoom);


        //Enable player
        playerRefs.stateMachine.ForceChangeState(playerRefs.IdleState);

    }

    public void ForceEndCutscene()
    {
        //Find the references
        CameraZoomController zoomer = GameObject.Find(Tags.CMvcam1).GetComponent<CameraZoomController>();
        Player_SwordRotationController swordRotation = GlobalPlayerReferences.Instance.references.swordRotation;
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;

        foreach (GameObject enemy in enemyRoomLogic.CurrentlySpawnedEnemies)
        {
            Enemy_References thisEnemyRefs = enemy.GetComponent<Enemy_References>();
            if(thisEnemyRefs.stateMachine.currentState.stateTag == StateTags.Agroo) { continue; }
            thisEnemyRefs.stateMachine.ChangeState(thisEnemyRefs.AgrooState);
        }
        swordRotation.FocusNewEnemy(enemyRoomLogic.CurrentlySpawnedEnemies[0].GetComponent<Enemy_References>().Focuseable);

        zoomer.RemoveZoomInfoAndUpdate("enterCutscene");
        TargetGroupSingleton.Instance.SetOnlyPlayerAndMouseTarget();
        playerRefs.stateMachine.ForceChangeState(playerRefs.IdleState);
    }
}
