using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoRooundProjectile_Spawner : MonoBehaviour
{
    [SerializeField] GameObject GoRoundPrefab;
    [SerializeField] Transform GoRoundRoot;
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            SpawnGoRound();
        }
    }
    public void SpawnGoRound()
    {
        Instantiate(GoRoundPrefab, GoRoundRoot.position, Quaternion.identity);
    }
}
