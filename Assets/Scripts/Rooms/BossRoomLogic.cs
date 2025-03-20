using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomLogic : RoomWithEnemiesLogic
{

    [SerializeField] bool dontEffectSkulls;
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
        if(gameState.FourDoors[BossIndex].isCompleted == false) { gameState.SkullsThatShouldBeUnlocked++; } //Aixo shauria de controlar millor plsss

        gameState.FourDoors[BossIndex].isCompleted = true;
        gameState.LastCompletedBoss = BossIndex;
        
        OnBossDefeated?.Invoke(BossIndex);
        gameState.justDefeatedBoss = true;

        //upgradesGroup.StartSpawnCutscene();

        isRoomPermanentlyCompleted = true;
    }
}
