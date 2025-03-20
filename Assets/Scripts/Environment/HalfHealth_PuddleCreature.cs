using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class HalfHealth_PuddleCreature : MonoBehaviour
{
    [SerializeField] SpriteShapeRenderer puddleShape;
    [SerializeField] PuddleCreatureLogiuc puddleCreature;
    [SerializeField] RoomWithEnemiesLogic roomWithEnemies;
    [SerializeField] float timeToAppearPuddle;
    [SerializeField] float delayBeforeAppearPuddle;
    [SerializeField] float delayBeforeAppearCreature;
    Enemy_HalfHealthSpecialAttack halfHealthAttack;
    PolygonCollider2D puddleCollider;

    private void OnEnable()
    {
        roomWithEnemies.onEnemiesSpawned += subscribeToHalfHealth;
        GameEvents.OnPlayerRespawned += restartState;
        puddleCollider = puddleShape.gameObject.GetComponent<PolygonCollider2D>();
        restartState();
    }
    private void OnDisable()
    {
        roomWithEnemies.onEnemiesSpawned -= subscribeToHalfHealth;
        GameEvents.OnPlayerRespawned -= restartState;
    }
    void subscribeToHalfHealth()
    {
        halfHealthAttack = roomWithEnemies.CurrentlySpawnedEnemies[0].GetComponent<Enemy_HalfHealthSpecialAttack>();
        //this should look for the boss_controller and get a reference when phase changing instead of this. HalfHealthAttack should be DELETED
    }
    void startSecondPhase()
    {
        StartCoroutine(appearPuddle());
    }
    IEnumerator appearPuddle()
    {
        yield return new WaitForSeconds(delayBeforeAppearPuddle);
        float timer = 0;
        Color baseColor = puddleShape.color;
        while (timer < timeToAppearPuddle)
        {
            timer += Time.deltaTime;
            puddleShape.color = new Color(
                baseColor.r,
                baseColor.g,
                baseColor.b,
                timer / timeToAppearPuddle
                );
            yield return null;
        }
        puddleShape.color = new Color(
                baseColor.r,
                baseColor.g,
                baseColor.b,
                1
                );
        yield return new WaitForSeconds(delayBeforeAppearCreature);
        puddleCollider.enabled = true;
    }
    void restartState()
    {
        puddleCollider.enabled = false;

        puddleShape.color = new Color(
               puddleShape.color.r,
               puddleShape.color.g,
               puddleShape.color.b,
               0
               );
    }
}
