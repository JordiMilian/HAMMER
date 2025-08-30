using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPlayerReferences : MonoBehaviour
{
     public Player_References references;
    public Transform playerTf;
    [HideInInspector] public static GlobalPlayerReferences Instance;
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
    public void SetPlayerReferences(GameObject player) //Called from ScenStarter
    {
        references = player.GetComponent<Player_References>();
        playerTf = player.transform;
    }

}
