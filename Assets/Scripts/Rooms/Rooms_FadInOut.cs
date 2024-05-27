using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Generic_OnTriggerEnterEvents;


public class Rooms_FadInOut : MonoBehaviour
{
    public GameObject[] RoomSpritesRoots;
    List<SpriteRenderer> AllRoomSprites = new List<SpriteRenderer>();
    [SerializeField] GameObject[] SpriteShapesRoots;

    [SerializeField] Generic_OnTriggerEnterEvents RoomTrigger;
    [SerializeField] Foregrounder DoorForegrounder;
    
    [SerializeField] float TransitionTime = 0.2f;
    [SerializeField] bool isStartingRoom;
    private void Awake()
    {
        foreach(GameObject root in RoomSpritesRoots)
        {
            SpriteRenderer[] thisRootsSprites = root.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer thisSprite in thisRootsSprites)
            {
                AllRoomSprites.Add(thisSprite);
            }
        }
    }
    private void Start()
    {
        if(isStartingRoom)
        {
            playerEnteredRoom(this, new EventArgsCollisionInfo(new Collider2D()));
        }
        else
        {
            playerExitedRoom(this, new EventArgsCollisionInfo(new Collider2D()));
        }
    }
    private void OnEnable()
    {
        RoomTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        RoomTrigger.OnTriggerEntered += playerEnteredRoom;
        RoomTrigger.OnTriggerExited += playerExitedRoom;
    }
    private void OnDisable()
    {
        RoomTrigger.OnTriggerEntered -= playerEnteredRoom;
        RoomTrigger.OnTriggerExited -= playerExitedRoom;
    }
    void playerEnteredRoom(object sender, EventArgsCollisionInfo args)
    {
        FadeIn(AllRoomSprites.ToArray());
        if (DoorForegrounder != null) { DoorForegrounder.CallTurnColor(); }
    }
    void playerExitedRoom(object sender, EventArgsCollisionInfo args)
    {
        FadeOut(AllRoomSprites.ToArray());
        if (DoorForegrounder != null) { DoorForegrounder.CallTurnBlack(); }
    }
    void FadeOut(SpriteRenderer[] sprites)
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            StartCoroutine(FadeOutSprite(sprite));
        }
    }
    IEnumerator FadeOutSprite(SpriteRenderer sprite)
    {
        float timer = 0;
        while(timer < TransitionTime)
        {
            timer += Time.deltaTime;
            float opacity = 1- (1 / TransitionTime * timer);
            sprite.color = new Color (1,1,1,opacity);
            yield return null;
        }
    }
    void FadeIn(SpriteRenderer[] sprites)
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            StartCoroutine(FadeInSprite(sprite));
        }
    }
    IEnumerator FadeInSprite(SpriteRenderer sprite)
    {
        float timer = 0;
        while (timer < TransitionTime)
        {
            timer += Time.deltaTime;
            float opacity =  1 / TransitionTime * timer;
            sprite.color = new Color(1, 1, 1, opacity);
            yield return null;
        }
        
    }
}
