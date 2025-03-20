using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class HalfHealth_PinkTraps : MonoBehaviour
{
    [SerializeField] List<GameObject> pinkTrapsList = new List<GameObject>();
    [SerializeField] List<GameObject> pinkSawsList = new List<GameObject>();
    [SerializeField] RoomWithEnemiesLogic roomWithEnemies;
    [SerializeField] float delayBeforeAppearingTraps;
    [SerializeField] float timeToAppearTraps;
    [Serializable]
    class trapInfo
    {
        public PinkTrap_Script Script;
        public SpriteRenderer SpriteRenderer;
        public GameObject IA_Obstacle;
        public trapInfo(PinkTrap_Script script, SpriteRenderer sprite, GameObject iA_Obstacle)
        {
            Script = script;
            SpriteRenderer = sprite;
            IA_Obstacle = iA_Obstacle;
        }
    }
    [Serializable]
    class sawInfo
    {
        public PinkSaw Script;
        public SpriteShapeRenderer Shape;
        public GameObject IA_Obstacle;
        public sawInfo(PinkSaw saw, SpriteShapeRenderer shape, GameObject iA_Obstacle)
        {
            Script = saw;
            Shape = shape;
            IA_Obstacle = iA_Obstacle;
        }
    }
    [SerializeField] List<trapInfo> trapInfosList = new List<trapInfo>();
    [SerializeField] List<sawInfo> sawInfosList = new List<sawInfo>();
    private void OnEnable()
    {
        roomWithEnemies.onEnemiesSpawned += subscribeToHalfHealth;
        GameEvents.OnPlayerRespawned += restartState;

        if (trapInfosList.Count != pinkTrapsList.Count)
        {
            trapInfosList.Clear();

            foreach (GameObject trapGO in pinkTrapsList)
            {
                trapInfosList.Add(new trapInfo(
                    trapGO.GetComponent<PinkTrap_Script>(),
                    trapGO.GetComponent<SpriteRenderer>(),
                    trapGO.GetComponentInChildren<CircleCollider2D>().gameObject
                    ));;
            }
        }
        if(sawInfosList.Count != pinkSawsList.Count)
        {
            sawInfosList.Clear();

            foreach (GameObject sawGO in pinkSawsList)
            {
                PinkSaw thisSaw = sawGO.GetComponent<PinkSaw>();

                sawInfosList.Add(new sawInfo(
                    thisSaw,
                    thisSaw.shapeController.gameObject.GetComponent<SpriteShapeRenderer>(),
                    thisSaw.GetComponentInChildren<CircleCollider2D>().gameObject
                ));
            }
        }
        restartState();
    }
    private void OnDisable()
    {
        roomWithEnemies.onEnemiesSpawned -= subscribeToHalfHealth;
        GameEvents.OnPlayerRespawned -= restartState;
    }
    void subscribeToHalfHealth()
    {
        roomWithEnemies.CurrentlySpawnedEnemies[0].GetComponent<Generic_StateMachine>().OnStateChanged += onBossStateChangeCheck;
    }
    void onBossStateChangeCheck(State newState)
    {
        if(newState.stateTag != StateTags.BossPhaseTransition) { return; }

        foreach(trapInfo trap in trapInfosList)
        {
            StartCoroutine(appearPinkTrap(trap));
        }
        foreach(sawInfo saw in sawInfosList)
        {
            StartCoroutine(appearPinkSaw(saw));
        }
    }
    IEnumerator appearPinkTrap(trapInfo trap)
    {
        yield return new WaitForSeconds(delayBeforeAppearingTraps);
        
        trap.SpriteRenderer.color = new Color(1, 1, 1, 0);
        float timer = 0;
        trap.IA_Obstacle.SetActive(true);
        while (timer < timeToAppearTraps)
        {
            timer += Time.deltaTime;
            trap.SpriteRenderer.color = new Color(1, 1, 1, timer / timeToAppearTraps);
            yield return null;
        }
        trap.SpriteRenderer.color = new Color(1, 1, 1, 1);
        trap.Script.enabled = true;
    }
    IEnumerator appearPinkSaw(sawInfo saw)
    {
        yield return new WaitForSeconds(delayBeforeAppearingTraps);

        saw.Shape.color = new Color (1,1,1,0);
        saw.IA_Obstacle.SetActive(true);
        float timer = 0;
        while (timer < timeToAppearTraps)
        {
            timer += Time.deltaTime;
            saw.Shape.color = new Color(1, 1, 1, timer / timeToAppearTraps);
            yield return null;
        }
        saw.Shape.color = new Color (1,1,1,1);
        saw.Script.isNotSawing = false;
    }
    void restartState()
    {
        foreach (trapInfo trap in trapInfosList)
        {
            trap.Script.enabled = false;
            trap.SpriteRenderer.color = new Color(1, 1, 1, 0);
            trap.IA_Obstacle.SetActive(false);
        }
        foreach(sawInfo saw in sawInfosList)
        {
            saw.Script.isNotSawing = true;
            saw.Shape.color = new Color(1, 1, 1, 0);
            saw.IA_Obstacle.SetActive(false);
        }
    }
}
