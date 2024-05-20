using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPlayerReferences : MonoBehaviour
{
     public Player_References references;
    [HideInInspector] public Transform playerTf;
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
        references = GameObject.Find(TagsCollection.MainCharacter).GetComponent<Player_References>();
        playerTf = references.transform;
    }

}
