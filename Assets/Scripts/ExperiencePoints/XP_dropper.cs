using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP_dropper : MonoBehaviour
{
    [SerializeField] List<XP_script> xpPrefabs = new List<XP_script>();

    [SerializeField] int xpToDrop;
    [SerializeField] bool triggerChoseXp;

    List<Vector2Int> sortedPrefabsBySize = new List<Vector2Int>(); //X = index in xpPrefabs, Y is amount Of Xp

    List<int> chosenXps = new List<int>();
    public void Awake()
    {
        sortPrefabsList();
    }
    public void Update()
    {
        if(triggerChoseXp)
        {
            spawnExperiences();
            triggerChoseXp = false;
        }
    }
    void sortPrefabsList()
    {
        for (int p = 0; p < xpPrefabs.Count; p++)
        {
            sortedPrefabsBySize.Add(new Vector2Int(p,xpPrefabs[p].XpAmount));
        }
        for (int s = 0; s < sortedPrefabsBySize.Count; s++)
        {
            int currentSmallestIndex = s;
            int currentSmallestAmount = sortedPrefabsBySize[s].y;
            for (int ss = s; ss < sortedPrefabsBySize.Count; ss++)
            {
                if (sortedPrefabsBySize[ss].y < currentSmallestAmount)
                {
                    currentSmallestIndex = ss;
                    currentSmallestAmount = sortedPrefabsBySize[ss].y;
                }
            }
            if(currentSmallestAmount < sortedPrefabsBySize[s].y)
            {
                swapElements(s, currentSmallestIndex);
            }
        }
        void swapElements(int indexA, int indexB)
        {
            Vector2Int middleMan = sortedPrefabsBySize[indexA];
            sortedPrefabsBySize[indexA] = sortedPrefabsBySize[indexB];
            sortedPrefabsBySize[indexB] = middleMan;
        }
    }
    public void spawnExperiences()
    {
        XP_script[] scriptsToSpawn = GetRandomXpScripts(xpToDrop);
        foreach(XP_script script in scriptsToSpawn)
        {
            GameObject newXpInstance = Instantiate(script.gameObject, transform.position, Quaternion.identity);
            newXpInstance.GetComponent<XP_script>().onSpawn();
        }
    }
    XP_script[] GetRandomXpScripts(int XpToDrop)
    {
        chosenXps.Clear();//for debugging, this list can be deleted
        List<XP_script> chosenPrefabs = new List<XP_script>();
        int countedXp = 0;
        while(countedXp < XpToDrop)
        {
            int largestIndex = sortedPrefabsBySize.Count- 1;
            int minAmountNeeded = XpToDrop - countedXp;
            for (int i = 0; i < sortedPrefabsBySize.Count; i++)
            {
                if(minAmountNeeded < sortedPrefabsBySize[i].y)
                {
                    largestIndex = i - 1;
                    break;
                }
            }
            if (largestIndex < 0) { Debug.LogWarning("No Prefab small enough for that amount"); break; }

            int randomIndex = UnityEngine.Random.Range(0, largestIndex+1);
            countedXp += sortedPrefabsBySize[randomIndex].y;
            chosenPrefabs.Add(xpPrefabs[sortedPrefabsBySize[randomIndex].x]);
            /*
            Debug.Log("Min required size: " + minAmountNeeded);
            Debug.Log("Largest posible prefab: " + sortedPrefabsBySize[largestIndex].y);
            Debug.Log("Added amount: " + sortedPrefabsBySize[randomIndex].y);
            */
            chosenXps.Add(sortedPrefabsBySize[randomIndex].y);

        }
        return chosenPrefabs.ToArray();
    }
}
