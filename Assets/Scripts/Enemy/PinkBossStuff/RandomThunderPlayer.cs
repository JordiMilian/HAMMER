using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomThunderPlayer : MonoBehaviour
{
    public bool isPlaying;

    [SerializeField] GameObject ThunderPrefab;
    [SerializeField] Collider2D SpawningArea;
    [SerializeField] RoomWithEnemiesLogic enemyRoomLogic;
    [SerializeField] float delayBetweenThunders;
    float elapsedTime = 0;
    
    private void OnEnable()
    {
        enemyRoomLogic.onRoomCompleted += StopThunders;
    }
    private void OnDisable()
    {
        enemyRoomLogic.onRoomCompleted -= StopThunders;
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
        isPlaying = true;
    }
}
