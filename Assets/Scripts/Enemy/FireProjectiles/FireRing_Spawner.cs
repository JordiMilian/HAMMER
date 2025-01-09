using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRing_Spawner : MonoBehaviour
{
    [SerializeField] GameObject FireRing_Prefab;
    Transform playerTarget;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            EV_SpawnFireRing();
        }
    }
    public void EV_SpawnFireRing()
    {
        if(playerTarget== null) { playerTarget = GlobalPlayerReferences.Instance.playerTf; }

        Instantiate(FireRing_Prefab,playerTarget.position,Quaternion.identity);
    }
}
