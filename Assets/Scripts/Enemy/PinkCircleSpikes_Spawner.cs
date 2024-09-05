using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkCircleSpikes_Spawner : MonoBehaviour
{
    [SerializeField] GameObject PinkCircleSpikes_Prefab;
    public void EV_SpawnCircleSpikes()
    {
        GameObject newSpikes = Instantiate(PinkCircleSpikes_Prefab,transform.position, Quaternion.identity);
        StartCoroutine(UsefullMethods.destroyWithDelay(2, newSpikes));
    }
}
