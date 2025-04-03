using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP_script : MonoBehaviour
{
    public int XpAmount;
    [SerializeField] Animator xpAnimator;
    [SerializeField] float minDistance, maxDistance, minTime, maxTime;
    [SerializeField] AnimationCurve spawnMovementCurve;
    [SerializeField] AudioClip pickedAudio;

    private void OnEnable()
    {
        GameEvents.OnPlayerRespawned += destroySelf;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerRespawned -= destroySelf;
    }
    public void onSpawn() //Called from XP_dropper
    {
        xpAnimator = GetComponent<Animator>();
        xpAnimator.speed = Random.Range(0.8f, 1.2f);
        xpAnimator.SetTrigger("spawn");
        float averageValueOfCurve = UsefullMethods.GetAverageValueOfCurve(spawnMovementCurve,10);
        StartCoroutine( UsefullMethods.ApplyCurveMovementOverTime(
            GetComponent<CharacterMover2>(),
            Random.Range(minDistance, maxDistance),
            Random.Range(minTime, maxTime),
            UsefullMethods.angle2Vector(Random.Range((float)0, (float)1) * Mathf.PI * 2),
            spawnMovementCurve,
            averageValueOfCurve
            ));
    }
    public void onPickedUp() //Called from player_experienceCollector
    {
        SFX_PlayerSingleton.Instance.playSFX(pickedAudio, 0.2f);
        GetComponent<CircleCollider2D>().enabled = false;
        xpAnimator.SetTrigger("pickedUp");
        StartCoroutine(UsefullMethods.destroyWithDelay(0.3f, gameObject));
    }
    void destroySelf()
    {
        Destroy(gameObject);
    }
    public void OnPlayerSpawn()
    {
        xpAnimator.SetTrigger("playerSpawn");
    }
}
