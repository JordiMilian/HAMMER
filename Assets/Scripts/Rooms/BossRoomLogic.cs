using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomLogic : RoomWithEnemiesLogic
{
    [SerializeField] GameState gameState;
    public Action<int> OnBossDefeated;
    public int BossIndex;
    //[SerializeField] UpgradesGroup upgradesGroup;

    private void Awake()
    {
        onRoomCompleted += BossDefeated;
    }
    void BossDefeated(BaseRoomWithDoorLogic roomLogic)
    {
        if(BossIndex < 0) { return; }
        gameState.FourDoors[BossIndex].isCompleted = true;
        gameState.LastCompletedBoss = BossIndex;
        gameState.SkullsThatShouldBeUnlocked++; //Aixo shauria de controlar millor plsss
        OnBossDefeated?.Invoke(BossIndex);
        gameState.justDefeatedBoss = true;

        //upgradesGroup.StartSpawnCutscene();

        isRoomPermanentlyCompleted = true;
    }
}
