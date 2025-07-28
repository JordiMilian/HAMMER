using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Respawning : PlayerState
{
    public override void OnEnable()
    {
        base.OnEnable();
        playerRefs.movement.SetMovementSpeed(SpeedsEnum.Stopped);
        playerRefs.swordRotation.SetRotationSpeed(SpeedsEnum.Stopped);


        //Set up weapon rotation
        Transform weaponPivot = playerRefs.weaponPivot.transform;
        weaponPivot.eulerAngles = new Vector3(
                    weaponPivot.transform.eulerAngles.x,
                    weaponPivot.transform.eulerAngles.y,
                    90
                    );

        playerRefs.hideSprites.HidePlayerSprites();

        //Move player to respawner position
        //TiedEnemy_Controller furthestRespawner =  RespawnersManager.Instance.GetFurthestActiveRespawner();

        TiedEnemy_Controller furthestRespawner =  FindObjectOfType<TiedEnemy_Controller>(); //FATALITY
        furthestRespawner.ActivateRespawner(false);
        furthestRespawner.MovePlayerHere(rootGameObject);

        //play respawner animation
        StartCoroutine(delayedPlayTiedAnimation());

        //respawner animation will call the EV_ from there to activate the player

        //
        IEnumerator delayedPlayTiedAnimation()
        {
            yield return new WaitForSeconds(1);
            furthestRespawner.PlayRespawningAnimation();
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if(currentCorotine != null) { StopCoroutine(currentCorotine); }
    }
    Coroutine currentCorotine;
    public void EV_ActuallyRespawn()
    {
        playerRefs.swordRotation.SetRotationSpeed(SpeedsEnum.VerySlow);
        playerRefs.GetComponent<IHealth>().RestoreAllHealth();
        playerRefs.hideSprites.ShowPlayerSprites();
        playerRefs.flasher.EndFlashing(0);
        currentCorotine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, playerRefs.IdleState, transitionTime_instant));
        CameraShake.Instance.ShakeCamera(IntensitiesEnum.Medium);
        GameEvents.OnPlayerRespawned?.Invoke();
    }
}
