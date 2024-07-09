using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnterTriggerCutscene;

public class BossRoomCutscene : BaseCutsceneLogic
{

    [SerializeField] RoomWithEnemiesLogic enemyRoomLogic;
    [SerializeField] float zoomToBoss;
    [SerializeField] AnimationClip bossEnterAnimationClip;
    [SerializeField] UI_BossHealthBar healthBar;
    public override void playThisCutscene()
    {
       currentCutscene = StartCoroutine(bossCutscene());
    }
    IEnumerator bossCutscene()
    {
        //Find the references
        CameraZoomController zoomer = GameObject.Find(TagsCollection.CMvcam1).GetComponent<CameraZoomController>();
        Transform bossTf = enemyRoomLogic.CurrentlySpawnedEnemies[0].transform;

        //Wait just in case for enemies to spawn
        yield return new WaitForSeconds(0.3f);

        //Zoom as intended
        zoomer.AddZoomInfoAndUpdate(new CameraZoomController.ZoomInfo(zoomToBoss, 3, "enterCutscene"));

        //Target the camera to boss
        TargetGroupSingleton.Instance.AddTarget(bossTf, 50, 1);

        //ACTIVATE ANIMATOR TRIGGER FOR INTENDED ANIMATION
        bossTf.gameObject.GetComponent<Animator>().SetTrigger("HitShield"); //PLACEHOLDER ALERT FUCKKKK

        yield return new WaitForSeconds(bossEnterAnimationClip.length);

        healthBar.ShowCanvas();

        yield return new WaitForSeconds(.5f);

        //return to basics
        zoomer.RemoveZoomInfoAndUpdate("enterCutscene");
        TargetGroupSingleton.Instance.RemoveTarget(bossTf);

        onCutsceneOver?.Invoke();
    }
}
