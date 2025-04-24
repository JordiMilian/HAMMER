using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemiesRoom : MonoBehaviour, IRoom, IRoomWithEnemies, ICutsceneable
{
    [Serializable]
    public class EnemySpawn
    {
        public GameObject PrefabEnemy;
        [Range(1, 2)]
        public int Tier = 1;
        public int Weight = 1;
        public float minInstances = 1;
        public float maxInstances = 1;
        [HideInInspector] public float currentInstances;
    }
    [SerializeField] int MaxWeight_T1;
    [SerializeField] int MaxWeight_T2;
    public List<EnemySpawn> SpawneableEnemies = new List<EnemySpawn>();

    [SerializeField] Collider2D SpawnArea;
    [SerializeField] GameObject EnemiesContainer;

    [SerializeField] DoorAnimationController EnterDoorAnimationController;
    [SerializeField] DoorAnimationController ExitDoorAnimationController;

    public Action OnAllEnemiesKilled { get; set; }
    public List<GameObject> CurrentlySpawnedEnemies { get; set; }

    public void OnRoomLoaded()
    {
        EnterDoorAnimationController.CloseDoor();
        SpawnEnemies();
        CutscenesManager.Instance.AddCutsceneable(this);
    }

    public void OnRoomUnloaded()
    {
        //destroy remaining enemies if there are?
        //also destroy the deadHeads
    }
    void SpawnEnemies()
    {
        CurrentlySpawnedEnemies = new List<GameObject>();

        if (SpawneableEnemies.Count == 0)
        {
            //Open door without cutsceneç
            ExitDoorAnimationController.EnableAutoDoorOpener();
            Debug.LogWarning("Nothing to spawn");
            return;
        }

        int currentWeight_T1 = 0;
        int currentWeight_T2 = 0;

        foreach (EnemySpawn spawn in SpawneableEnemies)
        {
            //Restart counters of spawners
            spawn.currentInstances = 0;

            //Spawn the minimums
            for (int i = 0; i < spawn.minInstances; i++)
            {
                ActuallySpawn(spawn);
                currentWeight_T1 += spawn.Weight;
            }
        }
        SpawnWeights(1, currentWeight_T1, MaxWeight_T1);
        SpawnWeights(2, currentWeight_T2, MaxWeight_T2);

        //
        //Spawn the enemies according to the weights
        void SpawnWeights(int Tier, int currentWeight, int maxWeight)
        {
            int attemptsToSpawn = 0;
            int weightReference = currentWeight;
            while (weightReference < maxWeight)
            {
                attemptsToSpawn++;
                if (attemptsToSpawn == 30)
                {
                    Debug.LogError("Something wrong with Spawners, check min-max stuff");
                    break;
                }
                //Pick a random index
                int randomIndex = UnityEngine.Random.Range(0, SpawneableEnemies.Count);
                if (SpawneableEnemies[randomIndex].Tier != Tier) { continue; } //If not in the proper Tier pick a diferent enemy
                EnemySpawn thisSpawn = SpawneableEnemies[randomIndex];

                //If already maxed, repeat
                if (thisSpawn.currentInstances >= thisSpawn.maxInstances) { continue; }

                //Spawn and add Weight
                ActuallySpawn(thisSpawn);
                weightReference += thisSpawn.Weight;
            }
        }
        void ActuallySpawn(EnemySpawn spawn)
        {
            // Find random point and Instantiate the Enemy
            Vector2 SpawnPosition = UsefullMethods.RandomPointInCollider(SpawnArea);

            GameObject SpawnedEnemy = Instantiate(
              spawn.PrefabEnemy,
              SpawnPosition,
              Quaternion.identity,
              EnemiesContainer.transform);

            CurrentlySpawnedEnemies.Add(SpawnedEnemy);

            //Subscribe to everything
            IDamageReceiver enemy_damageReceiver = SpawnedEnemy.GetComponent<IDamageReceiver>();
            IKilleable enemy_killeable = SpawnedEnemy.GetComponent<IKilleable>();
            IStats enemyStats = SpawnedEnemy.GetComponent<IStats>();

            enemy_killeable.OnKilled_event += EnemyDied;

            spawn.currentInstances++;

            //Set values per Death 
            /* DEPRECATED
            float hpPerDeath = UsefullMethods.normalizePercentage(gameState.enemiesPercentHealthPerDeath, false, true) * enemyStats.GetBaseStats().MaxHp;
            enemyStats.GetCurrentStats().MaxHp += gameState.playerDeaths * hpPerDeath;
            enemyStats.GetCurrentStats().CurrentHp += gameState.playerDeaths * hpPerDeath;

            float damagePerDeath = UsefullMethods.normalizePercentage(gameState.enemiesPercentDamageMultiplyPerDeath, false, true) * enemyStats.GetBaseStats().DamageMultiplicator;
            enemyStats.GetCurrentStats().DamageMultiplicator += gameState.playerDeaths * damagePerDeath;
            */
        }

    }
    void EnemyDied(DeadCharacterInfo info)
    {
        CurrentlySpawnedEnemies.Remove(info.DeadGameObject);
        //Maybe add slowMo checks in here
        if (CurrentlySpawnedEnemies.Count <= 0)
        {
            ExitDoorAnimationController.OpenWithCutscene();
            OnAllEnemiesKilled?.Invoke();
        }
    }
    #region ENTER ROOM CUTSCENEABLE
    [SerializeField] Transform CenterOfRoom;

    public IEnumerator ThisCutscene()
    {
        //Find the references
        CameraZoomController zoomer = GameObject.Find(Tags.CMvcam1).GetComponent<CameraZoomController>();
        Player_SwordRotationController swordRotation = GlobalPlayerReferences.Instance.references.swordRotation;
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;

        //Disable player 
        playerRefs.stateMachine.ForceChangeState(playerRefs.DisabledState);


        //Wait just in case for enemies to spawn
        yield return new WaitForSeconds(0.3f);

        //Look at center of room with zoom
        zoomer.AddZoomInfoAndUpdate(new CameraZoomController.ZoomInfo(6.5f, 3, "enterCutscene"));
        TargetGroupSingleton.Instance.AddTarget(CenterOfRoom, 20, 10);

        yield return new WaitForSeconds(0.9f);


        //Activate the Agroo of the enemies
        foreach (GameObject enemy in CurrentlySpawnedEnemies)
        {
            Generic_StateMachine stateMachine = enemy.GetComponent<Generic_StateMachine>();
            stateMachine.ChangeState(enemy.GetComponent<Enemy_References>().AgrooState);
        }

        swordRotation.FocusNewEnemy(CurrentlySpawnedEnemies[0].GetComponent<Enemy_References>().Focuseable);

        yield return new WaitForSeconds(0.5f);

        //Return to normal zoom
        zoomer.RemoveZoomInfoAndUpdate("enterCutscene");
        TargetGroupSingleton.Instance.RemoveTarget(CenterOfRoom);


        //Enable player
        playerRefs.stateMachine.ForceChangeState(playerRefs.IdleState);
    }
    public void ForceEndCutscene()
    {
        //Find the references
        CameraZoomController zoomer = GameObject.Find(Tags.CMvcam1).GetComponent<CameraZoomController>();
        Player_SwordRotationController swordRotation = GlobalPlayerReferences.Instance.references.swordRotation;
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;

        foreach (GameObject enemy in CurrentlySpawnedEnemies)
        {
            Enemy_References thisEnemyRefs = enemy.GetComponent<Enemy_References>();
            if (thisEnemyRefs.stateMachine.currentState.stateTag == StateTags.Agroo) { continue; }
            thisEnemyRefs.stateMachine.ChangeState(thisEnemyRefs.AgrooState);
        }
        swordRotation.FocusNewEnemy(CurrentlySpawnedEnemies[0].GetComponent<Enemy_References>().Focuseable);

        zoomer.RemoveZoomInfoAndUpdate("enterCutscene");
        TargetGroupSingleton.Instance.SetOnlyPlayerAndMouseTarget();
        playerRefs.stateMachine.ForceChangeState(playerRefs.IdleState);
    }
    #endregion
}
