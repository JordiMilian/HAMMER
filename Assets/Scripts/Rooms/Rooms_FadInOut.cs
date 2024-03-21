using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Generic_OnTriggerEnterEvents;


public class Rooms_FadInOut : MonoBehaviour
{
    SpriteRenderer[] RoomSpritesArray = new SpriteRenderer[0];
    [SerializeField] Generic_OnTriggerEnterEvents RoomTrigger;
    [SerializeField] Foregrounder DoorForegrounder;
    public GameObject RoomSprites;
    [SerializeField] float TransitionTime = 0.2f;
    [SerializeField] bool isStartingRoom;
    private void Awake()
    {
        RoomSpritesArray = RoomSprites.GetComponentsInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        if(!isStartingRoom)
        {
            FadeOut(RoomSpritesArray);
            DoorForegrounder.CallTurnBlack();
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
        FadeIn(RoomSpritesArray);
        DoorForegrounder.CallTurnColor();
    }
    void playerExitedRoom(object sender, EventArgsCollisionInfo args)
    {
        FadeOut(RoomSpritesArray);
        DoorForegrounder.CallTurnBlack();
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
