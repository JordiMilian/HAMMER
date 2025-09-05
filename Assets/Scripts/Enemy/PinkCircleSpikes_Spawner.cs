using UnityEngine;

public class PinkCircleSpikes_Spawner : MonoBehaviour
{
    [SerializeField] GameObject PinkCircleSpikes_Prefab;
    public void EV_SpawnCircleSpikes()
    {
        GameObject newSpikes = Instantiate(PinkCircleSpikes_Prefab,transform.position, Quaternion.identity);
        Destroy(newSpikes, 2f);
    }
}
