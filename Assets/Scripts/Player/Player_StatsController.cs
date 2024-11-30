using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StatsController : MonoBehaviour
{
   [SerializeField] Player_References playerRefs;
    Player_StatsV2 playerStats;
    private void Awake()
    {
        playerRefs.singlePointCollider.AddActivatorTag("Recompensa?¿¿?");
    }
    private void OnEnable()
    {
        GameEvents.OnPlayerRespawned += OnPlayerRespawned;
        playerRefs.singlePointCollider.OnTriggerEntered += OnPickedUpgrade;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerRespawned -= OnPlayerRespawned;
        playerRefs.singlePointCollider.OnTriggerEntered -= OnPickedUpgrade;
    }
    void OnPlayerRespawned()
    {
        //Aqui li falte molta cosa
    }
    void OnPickedUpgrade(Collider2D collider)
    {
        //Logica agafar un upgrade
    }
}
