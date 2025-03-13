using System.Collections.Generic;
using UnityEngine;

public class RespawnersManager : MonoBehaviour
{

    public List<Player_Respawner> Respawners = new List<Player_Respawner>();
    bool isSorted;
     Player_Respawner CurrentFurthestRespawner;
    //[SerializeField] GameObject PlayerGO;
    [SerializeField] GameState gameState;
    
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
    public void RespawnPlayer()
    {
        CheckFurthestRespawner();
        //CurrentFurthestRespawner.gameObject.GetComponent<TiedEnemy_StateMachine>().ShowBodies();
        CurrentFurthestRespawner.RespawnFromHere(GlobalPlayerReferences.Instance.references.gameObject); //Go to Player_Respawn
    }
    public void ForceSpawnInIndex(int indexByDistance)
    {
        if (indexByDistance > Respawners.Count) { Debug.LogError("respawner out of range"); return; }
        sortRespawners();
        Respawners[indexByDistance].ExternallyActivateRespawner();
        Respawners[indexByDistance].RespawnFromHere(GlobalPlayerReferences.Instance.references.gameObject);
    }
    void CheckFurthestRespawner()
    {
        CurrentFurthestRespawner = GetFurthestActiveRespawner();
    }
    
    public Player_Respawner GetFurthestActiveRespawner()
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
            //gameState.FurthestDoorsArray[roomGenerator.AreaIndex] = furthestIndex;
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

            Player_Respawner tempRespawner = Respawners[closestIndex];
            Respawners[closestIndex] = Respawners[i];
            Respawners[i] = tempRespawner;
        }
    }
    
    void setDistancesOfRespawners()
    {
        foreach (Player_Respawner respawner in Respawners)
        {
            respawner.distanceToManager = (respawner.transform.position - transform.position).sqrMagnitude;
        }
    }
}
