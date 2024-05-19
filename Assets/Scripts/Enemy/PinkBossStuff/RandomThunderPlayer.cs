using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomThunderPlayer : MonoBehaviour
{
    [SerializeField] GameObject ThunderPrefab;
    [SerializeField] Collider2D SpawningArea;
    [SerializeField] RoomWithEnemiesLogic enemyRoomLogic;
    [SerializeField] float startingDelay;
    [SerializeField] float MinDelay;
    [SerializeField] float accelerationRatePercent = 10;
    float elapsedTime = 0;
    float currentDelay;
    bool isPlaying;
    private void OnEnable()
    {
        enemyRoomLogic.onEnemiesSpawned += RestartDelays;
        enemyRoomLogic.onRoomCompleted += StopThunders;
    }
    private void OnDisable()
    {
        enemyRoomLogic.onEnemiesSpawned -= RestartDelays;
        enemyRoomLogic.onRoomCompleted -= StopThunders;
    }
    private void Start()
    {
        isPlaying = false;
    }
    private void Update()
    {
        if(!isPlaying) { return; }

        elapsedTime += Time.deltaTime;
        if(elapsedTime > currentDelay)
        {
            elapsedTime = 0;
            SpawnThunder();
            if(currentDelay > MinDelay) { currentDelay = currentDelay * (accelerationRatePercent / 100); }
        }
    }
    void SpawnThunder()
    {
        Vector2 thunderPos = UsefullMethods.RandomPointInCollider(SpawningArea);
        Instantiate(ThunderPrefab, thunderPos, Quaternion.identity);
    }
    public void RestartDelays()
    {
        currentDelay = startingDelay;
        elapsedTime = 0;
        isPlaying = true;
    }
    void StopThunders()
    {
        isPlaying = false;
    }
}
