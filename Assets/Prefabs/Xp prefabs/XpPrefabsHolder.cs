using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Xp Prefabs Holder", menuName = "Xp Prefabs Holder")]
public class XpPrefabsHolder : ScriptableObject
{
    public List<XP_script> xpPrefabs = new List<XP_script>();
    public List<Vector2Int> sortedPrefabsBySize = new List<Vector2Int>();//X = index in xpPrefabs, Y is amount Of Xp

    public bool isUnsorted()
    {
        int lastValue = 0;
        if(sortedPrefabsBySize.Count < xpPrefabs.Count) { return true; }
        for (int i = 0; i < sortedPrefabsBySize.Count; i++)
        {
            if (sortedPrefabsBySize[i].y < lastValue) { return true; }
            lastValue = sortedPrefabsBySize[i].y;
        }
        return false;
    }
    public void sortPrefabsList()
    {
        for (int p = 0; p < xpPrefabs.Count; p++)
        {
            sortedPrefabsBySize.Add(new Vector2Int(p, xpPrefabs[p].XpAmount));
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
            if (currentSmallestAmount < sortedPrefabsBySize[s].y)
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
}
