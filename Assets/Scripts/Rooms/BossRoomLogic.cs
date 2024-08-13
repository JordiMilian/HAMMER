using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomLogic : RoomWithEnemiesLogic
{
    [SerializeField] GameState gameState;
    public Action<int> OnBossDefeated;
    public int BossIndex;
    [SerializeField] UpgradesGroup upgradesGroup;

    private void Awake()
    {
        onRoomCompleted += BossDefeated;
    }
    void BossDefeated(BaseRoomWithDoorLogic roomLogic)
    {
        gameState.FourDoors[BossIndex].isCompleted = true;
        gameState.LastCompletedIndex = BossIndex;
        OnBossDefeated?.Invoke(BossIndex);

        upgradesGroup.StartSpawnCutscene();

        isRoomPermanentlyCompleted = true;
    }
}
