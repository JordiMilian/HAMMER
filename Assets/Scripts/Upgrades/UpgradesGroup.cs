using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesGroup : MonoBehaviour
{
    public Action CallSpawnUpgrades;
    [SerializeField] BaseRoomWithDoorLogic roomLogic;
    [SerializeField] int amountOfContainers;
    int tempAmountOfContainers;
    [SerializeField] float radiusToSpawn;
    [SerializeField] bool trigger_TestSpawnContainers;

    [SerializeField] bool avoidUpgradesRepetition;

    [SerializeField] Upgrades_AvailableUpgrades availableUpgrades;
    List<Upgrade> SelectedUpgrades = new List<Upgrade>();
    [SerializeField] GameObject base_UpgradeContainer;
    [SerializeField] List<UpgradeContainer> spawnedUpgradesContainers = new List<UpgradeContainer>();

    private void Update()
    {
        if(trigger_TestSpawnContainers)
        {
            onSpawnNewContainers();
            trigger_TestSpawnContainers = false;
        }
    }
    private void OnEnable()
    {
        CallSpawnUpgrades += onSpawnNewContainers;
    }
    private void OnDisable()
    {
        CallSpawnUpgrades -= onSpawnNewContainers;
    }
    void onSpawnNewContainers()
    {
        RemoveSpawnedContainers();

        tempAmountOfContainers = amountOfContainers;
        SelectedUpgrades.Clear();
        if (avoidUpgradesRepetition) { SelectedUpgrades = NonRepeatingSelection(tempAmountOfContainers); }
        else { SelectedUpgrades= RepeatingSelection(tempAmountOfContainers); }
        tempAmountOfContainers = SelectedUpgrades.Count;


        Vector2[] prefabPositions = UsefullMethods.GetPolygonPositions(transform.position, tempAmountOfContainers, radiusToSpawn);

        for (int i = 0; i < tempAmountOfContainers; i++)
        {
            
            GameObject newContainer = Instantiate(base_UpgradeContainer, prefabPositions[i], Quaternion.identity, transform);
            UpgradeContainer containerScript = newContainer.GetComponent<UpgradeContainer>();

            containerScript.upgradeEffect = SelectedUpgrades[i]; //Add upgrade effect 
            containerScript.IndexInGroup = i; //save index in script
            containerScript.OnSpawnContainer(); //OnSpawn this container
            spawnedUpgradesContainers.Add(containerScript); //add to list
            spawnedUpgradesContainers[i].OnPickedUp += OnPickedOneUpgrade; //subscribe to picked up (desubscribed in DispawnCurrentContaoiners())

        }
    }
    List<Upgrade> NonRepeatingSelection(int amount)
    {

        List<Upgrade> selected = new List<Upgrade>();

        int availableUpgradesCount = availableUpgrades.AvailableUpgrades.Count;
        if (amount > availableUpgradesCount)
        {
            amount = availableUpgradesCount; //this should go to the temp amount BUG
        }

        int attempts = 0;
        for (int i = 0; i < amount; i++)
        {
            Upgrade maybeUpdate = availableUpgrades.GetRandomUpgrade();

            if (!selected.Contains(maybeUpdate)) { selected.Add(maybeUpdate); continue; } //if that upgrade is not contained continue from here

            i--;
            attempts++;
            if(attempts > 30) { Debug.LogError("Something wrong with Available Upgrades, pls check if there are repeating Upgrades"); break; }
        }

        return selected;
    }
    List<Upgrade> RepeatingSelection(int amount)
    {
        List<Upgrade> selected = new List<Upgrade>();

        for (int i = 0; i < amount; i++)
        {
            Upgrade maybeUpdate = availableUpgrades.GetRandomUpgrade();
            selected.Add(maybeUpdate);
        }
        return selected;
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
