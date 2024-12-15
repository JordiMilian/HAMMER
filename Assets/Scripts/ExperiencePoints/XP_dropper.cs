using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP_dropper : MonoBehaviour
{
    [SerializeField] XpPrefabsHolder prefabs;

    [SerializeField] int testingXpToDrop;
    [SerializeField] bool triggerChoseXp;


    List<int> chosenXps = new List<int>();
    public void Update()
    {
        if(triggerChoseXp)
        {
            spawnExperiences(testingXpToDrop);
            triggerChoseXp = false;
        }
    }
    public GameObject[] spawnExperiences(int xpToDrop)
    {
        Debug.Log("spawn xp????" + xpToDrop);
        if (prefabs.isUnsorted()) { prefabs.sortPrefabsList(); }

        XP_script[] scriptsToSpawn = GetRandomXpScripts(xpToDrop);
        List<GameObject> spawnedXps = new List<GameObject>();
        foreach(XP_script script in scriptsToSpawn)
        {
            GameObject newXpInstance = Instantiate(script.gameObject, transform.position, Quaternion.identity);
            newXpInstance.GetComponent<XP_script>().onSpawn();
            spawnedXps.Add(newXpInstance);
        }
        return spawnedXps.ToArray();

        //
        XP_script[] GetRandomXpScripts(int XpToDrop)
        {
            chosenXps.Clear();//for debugging, this list can be deleted
            List<XP_script> chosenPrefabs = new List<XP_script>();
            int countedXp = 0;
            while (countedXp < XpToDrop)
            {
                int largestIndex = prefabs.sortedPrefabsBySize.Count - 1;
                int minAmountNeeded = XpToDrop - countedXp;
                for (int i = 0; i < prefabs.sortedPrefabsBySize.Count; i++)
                {
                    if (minAmountNeeded < prefabs.sortedPrefabsBySize[i].y)
                    {
                        largestIndex = i - 1;
                        break;
                    }
                }
                if (largestIndex < 0) { Debug.LogWarning("No Prefab small enough for that amount"); break; }

                int randomIndex = UnityEngine.Random.Range(0, largestIndex + 1);
                countedXp += prefabs.sortedPrefabsBySize[randomIndex].y;
                chosenPrefabs.Add(prefabs.xpPrefabs[prefabs.sortedPrefabsBySize[randomIndex].x]);
                
                //Debug.Log("Min required size: " + minAmountNeeded);
                //Debug.Log("Largest posible prefab: " + prefabs.sortedPrefabsBySize[largestIndex].y);
                //Debug.Log("Added amount: " + prefabs.sortedPrefabsBySize[randomIndex].y);
                
                chosenXps.Add(prefabs.sortedPrefabsBySize[randomIndex].y);

            }
            return chosenPrefabs.ToArray();
        }
    }
    
}
