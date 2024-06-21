using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesGroup : MonoBehaviour
{ 

    [SerializeField] BaseRoomWithDoorLogic doorLogic;
    [SerializeField] int amountOfContainers;
    int tempAmountOfContainers;
    [SerializeField] float radiusToSpawn;
    [SerializeField] bool trigger_TestSpawnContainers;

    [SerializeField] bool avoidUpgradesRepetition;
    
    List<GameObject> SelectedPrefabs = new List<GameObject>();
    [SerializeField] List<UpgradeContainer> spawnedUpgradesContainers = new List<UpgradeContainer>();

    private void Update()
    {
        if(trigger_TestSpawnContainers)
        {
            onSpawnNewContainers(new BaseRoomWithDoorLogic());
            trigger_TestSpawnContainers = false;
        }
    }
    private void OnEnable()
    {
        if(doorLogic != null) { doorLogic.onRoomCompleted += onSpawnNewContainers; }
        
    }
    private void OnDisable()
    {
        if (doorLogic != null) { doorLogic.onRoomCompleted -= onSpawnNewContainers; }
    }
    void onSpawnNewContainers(BaseRoomWithDoorLogic thisLogic)
    {
        RemoveSpawnedContainers();

        tempAmountOfContainers = amountOfContainers;
        if (avoidUpgradesRepetition) { NonRepeatingSelection(); }
        else { RepeatingSelection(); }


        Vector2[] prefabPositions = UsefullMethods.GetPolygonPositions(transform.position, tempAmountOfContainers, radiusToSpawn);

        for (int i = 0; i < tempAmountOfContainers; i++)
        {
            GameObject newContainer = Instantiate(SelectedPrefabs[i], prefabPositions[i], Quaternion.identity, transform);
            UpgradeContainer containerScript = newContainer.GetComponent<UpgradeContainer>();


            containerScript.IndexInGroup = i; //save index in script
            containerScript.OnSpawnContainer(); //OnSpawn this container
            spawnedUpgradesContainers.Add(containerScript); //add to list
            spawnedUpgradesContainers[i].OnPickedUp += OnPickedOneUpgrade; //subscribe to picked up (desubscribed in DispawnCurrentContaoiners())

        }
    }
    void NonRepeatingSelection()
    {
        SelectedPrefabs.Clear();

        int availableUpgradesCount = Upgrades_AvailableUpgrades.Instance.AvailableUpgrades.Count;
        if (tempAmountOfContainers > availableUpgradesCount)
        {
            tempAmountOfContainers = availableUpgradesCount;
        }

        int attempts = 0;
        for (int i = 0; i < tempAmountOfContainers; i++)
        {
            GameObject maybeUpdate = Upgrades_AvailableUpgrades.Instance.GetRandomUpgrade();

            if (!SelectedPrefabs.Contains(maybeUpdate)) { SelectedPrefabs.Add(maybeUpdate); continue; }
            i--;
            attempts++;
            if(attempts > 30) { Debug.LogError("Something wrong with Available Upgrades, pls check if there are repeating Upgrades in Singleton"); break; }
        }
    }
    void RepeatingSelection()
    {
        SelectedPrefabs.Clear();

        for (int i = 0; i < tempAmountOfContainers; i++)
        {
            GameObject maybeUpdate = Upgrades_AvailableUpgrades.Instance.GetRandomUpgrade();
            SelectedPrefabs.Add(maybeUpdate);
        }
    }

    void OnPickedOneUpgrade(int selectedIndex)
    {
        RemoveSpawnedContainers(selectedIndex);
    }
    void RemoveSpawnedContainers(int exeptionIndex = -1)
    {
        for (int i = 0; i < spawnedUpgradesContainers.Count; i++)
        {
            spawnedUpgradesContainers[i].OnPickedUp -= OnPickedOneUpgrade;
            if (i == exeptionIndex) { continue; }

            spawnedUpgradesContainers[i].OnDispawnContainer();
        }
        spawnedUpgradesContainers.Clear();
    }
  
}
