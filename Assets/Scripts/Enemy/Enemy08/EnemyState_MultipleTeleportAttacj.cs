using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyState_MultipleTeleportAttacj : EnemyState_Attack
{
    [Header("Visuals")]
    [SerializeField] Color flashColor = Color.green;
    [SerializeField] AudioClip teleportSound;
    [SerializeField] VisualEffect teleportEffect;
    [Serializable]
    struct teleportAttackInfo
    {
        public string AnimatorStateName;
        public float RadiusToTeleport;
    }
    [SerializeField] teleportAttackInfo[] teleportAttacks;
    Coroutine teleportCoroutine;
    public override void OnEnable()
    {
        base.OnEnable();

        EnemyRefs.moveToTarget.SetMovementSpeed(SpeedsEnum.VerySlow);
        teleportCoroutine = StartCoroutine(TeleportAttackCoroutine());
    }
    public override void OnDisable()
    {
        base.OnDisable();
        if (teleportCoroutine != null) { StopCoroutine(teleportCoroutine); }
    }
    IEnumerator TeleportAttackCoroutine()
    {
        const float addedPitchPerTeleport = 0.05f;
        float addedPitch = 0;

        for (int i = 0; i < teleportAttacks.Length; i++)
        {
            teleportAttackInfo info = teleportAttacks[i];

            animator.CrossFade(info.AnimatorStateName, 0.1f);
            yield return WaitForNextAnimationToFinish();

            if(i == teleportAttacks.Length -1)
            {
                OnAttackFinished();
            }
            else
            {
                TeleportAroundPlayer(info.RadiusToTeleport);
                SFX_PlayerSingleton.Instance.playSFX(teleportSound,0,0,addedPitch);
                addedPitch += addedPitchPerTeleport;
                teleportEffect.Play();
                EnemyRefs.flasher.CallCustomFlash(.8f, flashColor);
            }
   
        }
    }

    void TeleportAroundPlayer(float radius)
    {
        const float minDistanceToTeleport = 1.5f;

        Vector2 playerPos = GlobalPlayerReferences.Instance.playerTf.position;
        Vector2 startingPos = rootGameObject.transform.position;
        Vector2 randomCirclePos;
        do
        {
            randomCirclePos = UnityEngine.Random.insideUnitCircle.normalized * radius;
        }
        while (((playerPos + randomCirclePos) - startingPos).magnitude < minDistanceToTeleport);


        Vector2 teleportPosition = playerPos + randomCirclePos;
        rootGameObject.transform.position = teleportPosition;
    }
}
