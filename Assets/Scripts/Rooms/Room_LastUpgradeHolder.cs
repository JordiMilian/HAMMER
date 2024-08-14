using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_LastUpgradeHolder : BaseCutsceneLogic
{
    //SERIELIZEFIELD
    [SerializeField] BaseRoomWithDoorLogic roomLogic;
    [SerializeField] Room_script thisRoom;
    [SerializeField] GameState gameState;
    [SerializeField] GameObject BaseUpgradeContainerPrefab;
    [SerializeField] Transform SpawnPosition;
    [SerializeField] RoomWithEnemiesLogic roomWithEnemies;

    //An instance of this script is holded by every room. 
    //The subcription to itselt is done by the MANAGER
    //The unsubscribtion is done by itself by multiple reasons (player picks up the uprade or player dies again)
    private void OnEnable()
    {
        //roomLogic.onRoomCompleted += checkIfSpawnUpgrade;
        roomWithEnemies.onEnemiesSpawned += checkIfSpawnUpgrade;
    }
    private void OnDisable()
    {
        roomWithEnemies.onEnemiesSpawned -= checkIfSpawnUpgrade;
        //roomLogic.onRoomCompleted -= checkIfSpawnUpgrade;
    }

    void checkIfSpawnUpgrade( )
    {
        if (!gameState.isLostUpgradeAvailable) { return; }

        if(thisRoom.indexInCompleteList == gameState.IndexOfLostUpgradeRoom)
        {
            CutscenesManager.Instance.AddCutscene(this);

            gameState.isLostUpgradeAvailable = false;
        }
    }
    public override void playThisCutscene()
    {
        //currentCutscene = StartCoroutine(spawnCutscene()); //Una guarrada que este script sigue herencia de BaseCutscene pero fuck it
        StartCoroutine( spawnWithoutCutscene());
    }
    IEnumerator spawnCutscene()
    {
        TargetGroupSingleton.Instance.AddTarget(SpawnPosition, 10, 1);
        yield return new WaitForSeconds(.7f);

        UpgradeContainer thisUpgrade = SpawnUpgrades();

        yield return new WaitForSeconds(UsefullMethods.getCurrentAnimationLenght(thisUpgrade.GetComponent<Animator>()) + 1f);

        TargetGroupSingleton.Instance.RemoveTarget(SpawnPosition);

        onCutsceneOver?.Invoke();
    }
    UpgradeContainer SpawnUpgrades()
    {

        GameObject newContainer = Instantiate(BaseUpgradeContainerPrefab, SpawnPosition.position, Quaternion.identity, SpawnPosition);
        UpgradeContainer upgradeContainer = newContainer.GetComponent<UpgradeContainer>();

        upgradeContainer.upgradeEffect = gameState.lastLostUpgrade;

        upgradeContainer.OnSpawnContainer();

        return upgradeContainer;
    }
    IEnumerator spawnWithoutCutscene()
    {
        UpgradeContainer thisUpgrade = SpawnUpgrades();
        yield return new WaitForSeconds(0.1f);
        onCutsceneOver?.Invoke();
    }
   
}
