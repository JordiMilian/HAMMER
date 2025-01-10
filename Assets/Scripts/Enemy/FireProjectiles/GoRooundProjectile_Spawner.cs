using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoRooundProjectile_Spawner : MonoBehaviour
{
    [SerializeField] GameObject GoRoundPrefab;
    [SerializeField] Transform GoRoundRoot;
    [SerializeField] int TestingCircleAmount;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            EV_MultipleGoRoundSpawns(TestingCircleAmount);
        }
    }
    public void EV_MultipleGoRoundSpawns(int amount)
    {
        float DegPerInstance = 360 / (float)amount;
        float acumulativeDeg = 0;
        for (int i = 0; i < amount; i++)
        {
           GameObject instance = Instantiate(GoRoundPrefab, GoRoundRoot.position, Quaternion.identity, GoRoundRoot);
           instance.GetComponentInChildren<GoRoundProjectile_Controller>().SetStartingRotation(acumulativeDeg);
           acumulativeDeg += DegPerInstance;
        }
    }
}
