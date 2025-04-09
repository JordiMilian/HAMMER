using System.Collections.Generic;
using UnityEngine;

public class RespawnersManager : MonoBehaviour
{

    public List<TiedEnemy_Controller> Respawners = new List<TiedEnemy_Controller>();
    bool isSorted;

    #region SINGLETON 
    public static RespawnersManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion
    public TiedEnemy_Controller GetRespawnerByIndexOfDistance(int indexByDistance)
    {
        if (indexByDistance > Respawners.Count) { Debug.LogError("respawner out of range"); return null; }
        if (!isSorted) { sortRespawners(); isSorted = true; }

        return Respawners[indexByDistance];
    }
    public TiedEnemy_Controller GetFurthestActiveRespawner()
    {
        if (!isSorted)
        {
            sortRespawners();
            isSorted = true;
        }
        return Respawners[GetFurthestActiveRespawnerIndex()];

        //
        int GetFurthestActiveRespawnerIndex()
        {
            int furthestIndex = 0;
            for (int i = 0; i < Respawners.Count; i++)
            {
                if (Respawners[i].IsActivated)
                {
                    furthestIndex = i;
                }
            }
            return furthestIndex;
        }
    }
    
    void sortRespawners()
    {
        setDistancesOfRespawners();

        int respawneresLenght = Respawners.Count;
        for (int i = 0; i < respawneresLenght; i++)
        {
            int closestIndex = i;
            for (int j = i +1; j < respawneresLenght; j++)
            {
                if (Respawners[j].distanceToManager < Respawners[closestIndex].distanceToManager)
                {
                    closestIndex = j;
                }
            }

            TiedEnemy_Controller tempRespawner = Respawners[closestIndex];
            Respawners[closestIndex] = Respawners[i];
            Respawners[i] = tempRespawner;
        }
    }
    void setDistancesOfRespawners()
    {
        foreach (TiedEnemy_Controller respawner in Respawners)
        {
            respawner.distanceToManager = (respawner.transform.position - transform.position).sqrMagnitude;
        }
    }
}
