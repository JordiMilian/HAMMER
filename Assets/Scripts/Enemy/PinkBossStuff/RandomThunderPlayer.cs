using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomThunderPlayer : MonoBehaviour
{
    /*
    public bool isPlaying;

    [SerializeField] GameObject ThunderPrefab;
    [SerializeField] Collider2D SpawningArea;
    [SerializeField] RoomWithEnemiesLogic enemyRoomLogic;
    [SerializeField] float delayBetweenThunders;
    float elapsedTime = 0;
    Enemy_HalfHealthSpecialAttack halfHealth;
    
    private void OnEnable()
    {
        enemyRoomLogic.onRoomCompleted += StopThunders;
        enemyRoomLogic.onEnemiesSpawned += subscribeToHalfHealth;
    }
    void subscribeToHalfHealth()
    {
        halfHealth = enemyRoomLogic.CurrentlySpawnedEnemies[0].GetComponent<Enemy_HalfHealthSpecialAttack>();
        halfHealth.OnChangePhase += StartThunders;
    }
    private void OnDisable()
    {
        enemyRoomLogic.onRoomCompleted -= StopThunders;
        enemyRoomLogic.onEnemiesSpawned -= subscribeToHalfHealth;
    }
    private void Update()
    {
        if(!isPlaying) { return; }

        elapsedTime += Time.deltaTime;
        if(elapsedTime > delayBetweenThunders)
        {
            elapsedTime = 0;
            SpawnThunder();
        }
    }
    void SpawnThunder()
    {
        Vector2 thunderPos = UsefullMethods.RandomPointInCollider(SpawningArea);
        Instantiate(ThunderPrefab, thunderPos, Quaternion.identity);
    }
    void StopThunders(BaseRoomWithDoorLogic door)
    {
        isPlaying = false;
    }
    void StartThunders()
    {
        halfHealth.OnChangePhase -= StartThunders;
        isPlaying = true;
    }
    */
}
