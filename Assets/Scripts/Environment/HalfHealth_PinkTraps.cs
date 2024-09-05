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
        public trapInfo(PinkTrap_Script script, SpriteRenderer sprite)
        {
            Script = script;
            SpriteRenderer = sprite;
        }
    }
    [Serializable]
    class sawInfo
    {
        public PinkSaw Script;
        public SpriteShapeRenderer Shape;
        public sawInfo(PinkSaw saw, SpriteShapeRenderer shape)
        {
            Script = saw;
            Shape = shape;
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
                    trapGO.GetComponent<SpriteRenderer>()
                    ));
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
                    thisSaw.shapeController.gameObject.GetComponent<SpriteShapeRenderer>()
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
        roomWithEnemies.CurrentlySpawnedEnemies[0].GetComponent<Enemy_HalfHealthSpecialAttack>().OnChangePhase += startSecondPhase;
    }
    void startSecondPhase()
    {
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
        float timer = 0;
        while (timer < timeToAppearTraps)
        {
            timer += Time.deltaTime;
            saw.Shape.color = new Color(1, 1, 1, timer / timeToAppearTraps);
            yield return null;
        }
        saw.Shape.color = new Color (1,1,1,1);
        saw.Script.enabled = true;
    }
    void restartState()
    {
        foreach (trapInfo trap in trapInfosList)
        {
            trap.Script.enabled = false;
            trap.SpriteRenderer.color = new Color(1, 1, 1, 0);
        }
        foreach(sawInfo saw in sawInfosList)
        {
            saw.Script.enabled = false;
            saw.Shape.color = new Color(1, 1, 1, 0);
        }
    }
}
