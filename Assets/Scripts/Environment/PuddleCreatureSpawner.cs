using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PuddleCreatureSpawner : MonoBehaviour
{
    [SerializeField] SpriteShapeRenderer puddleShape;
    [SerializeField] PuddleCreatureLogiuc puddleCreature;
    [SerializeField] GameObject roomWithEnemies_interfaceHolder;
    [SerializeField] IRoomWithEnemies roomWithEnemies;
    [SerializeField] float timeToAppearPuddle;
    [SerializeField] float delayBeforeAppearPuddle;
    [SerializeField] float delayBeforeAppearCreature;
    PolygonCollider2D puddleCollider;

    private void OnValidate()
    {
        UsefullMethods.CheckIfGameobjectImplementsInterface<IRoomWithEnemies>(ref roomWithEnemies_interfaceHolder, ref roomWithEnemies);
    }
    private void OnEnable()
    {
        puddleCollider = puddleShape.gameObject.GetComponent<PolygonCollider2D>();
        GameEvents.OnPlayerRespawned += restartState;
    }
    private void Start()
    {
        restartState();
    }
    public void AppearPuddleAndCreature()
    {
        StartCoroutine(appearPuddle());
    }
     IEnumerator appearPuddle()
    {
        Debug.Log("start cutscene");
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
