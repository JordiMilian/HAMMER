using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato_Spawner : MonoBehaviour
{
    public float TimeBetweenTomatoes;
    float Timer;
    public GameObject TomatoPrefab;
    bool SpawnTomatoes;
    public Transform TomatoFollowPlayer;
    

    void Start()
    {
        //TomatoFollowPlayer = GetComponentInChildren<Tomato_FollowPlayer>().gameObject.transform;
        Timer = TimeBetweenTomatoes;
        
    }

    
    void Update()
    {
        if (SpawnTomatoes)
        {
            Timer = Timer - Time.deltaTime;
            if (Timer <= 0)
            {
                SpawnTomato();
                Timer = TimeBetweenTomatoes;
            }
        }
    }
    public void SpawnTomato()
    {
        var Tomato = Instantiate(TomatoPrefab,TomatoFollowPlayer.position,TomatoFollowPlayer.rotation);
    }
    
}
