using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static Generic_OnTriggerEnterEvents;


public class Rooms_FadInOut : MonoBehaviour
{
    public GameObject[] RoomSpritesRoots;
    List<SpriteRenderer> AllRoomSprites = new List<SpriteRenderer>();
    List<SpriteShapeRenderer> AllRoomShapes = new List<SpriteShapeRenderer>();
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
        
        foreach (GameObject root in SpriteShapesRoots)
        {
            SpriteShapeRenderer[] thisRootsShapes = root.GetComponentsInChildren<SpriteShapeRenderer>();
            foreach (SpriteShapeRenderer thisShape in thisRootsShapes)
            {
                AllRoomShapes.Add(thisShape);
            }
        }
        
    }
    private void Start()
    {
        if(isStartingRoom)
        {
            playerEnteredRoom(new Collider2D());
        }
        else
        {
            playerExitedRoom(new Collider2D());
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
    void playerEnteredRoom(Collider2D collision)
    {
        FadeIn();
        if (DoorForegrounder != null) { DoorForegrounder.CallTurnColor(); }
    }
    void playerExitedRoom(Collider2D collision)
    {
        FadeOut();
        if (DoorForegrounder != null) { DoorForegrounder.CallTurnBlack(); }
    }
    void FadeOut()
    {
        foreach (SpriteRenderer sprite in AllRoomSprites)
        {
            StartCoroutine(FadeOutSprite(result => sprite.color = result));
        }
        foreach(SpriteShapeRenderer shape in AllRoomShapes)
        {
            StartCoroutine(FadeOutSprite(result => shape.color = result));
        }
    }
    IEnumerator FadeOutSprite(Action<Color> colorToChange)
    {
        float timer = 0;
        while(timer < TransitionTime)
        {
            timer += Time.deltaTime;
            float opacity = 1- (1 / TransitionTime * timer);
            Color newColor = new Color (1,1,1,opacity);
            colorToChange(newColor);
            yield return null;
        }
    }
    void FadeIn()
    {
        foreach (SpriteRenderer sprite in AllRoomSprites)
        {
            StartCoroutine(FadeInSprite(result => sprite.color = result));
        }
        foreach(SpriteShapeRenderer shape in AllRoomShapes)
        {
            StartCoroutine(FadeInSprite(result => shape.color = result));
        }
    }
    IEnumerator FadeInSprite(Action<Color> colorToChange)
    {
        float timer = 0;
        while (timer < TransitionTime)
        {
            timer += Time.deltaTime;
            float opacity =  1 / TransitionTime * timer;
            Color newColor = new Color(1, 1, 1, opacity);
            colorToChange(newColor);
            yield return null;
        }
        
    }

}

