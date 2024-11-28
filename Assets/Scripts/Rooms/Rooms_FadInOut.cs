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
    List<Color> rootsColors = new List<Color>();

    [SerializeField] Generic_OnTriggerEnterEvents RoomTrigger;
    [SerializeField] Foregrounder DoorForegrounder;
    
    [SerializeField] float TransitionTime = 0.2f;
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

        Rooms_FadeInOut_StartingRoomsCheck.Instance.roomsFades.Add(this);
        
    }
    public void checkCurrentRoom()
    {
        Vector2 playerPos = GlobalPlayerReferences.Instance.playerTf.position;
        Collider2D roomCollider = RoomTrigger.GetComponent<Collider2D>();

        if (roomCollider.OverlapPoint(playerPos))
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
            StartCoroutine(FadeOutSprite(result => sprite.color = result, sprite.color, sprite));
        }
        foreach(SpriteShapeRenderer shape in AllRoomShapes)
        {
            StartCoroutine(FadeOutSprite(result => shape.color = result, shape.color, shape));
        }
    }
    IEnumerator FadeOutSprite(Action<Color> colorToChange, Color baseColor, Renderer renderer)
    {
        float timer = 0;
        while(timer < TransitionTime)
        {
            timer += Time.deltaTime;
            float opacity = 1- (1 / TransitionTime * timer);
            Color newColor = new Color (baseColor.r, baseColor.g, baseColor.b, opacity);
            colorToChange(newColor);
            yield return null;
        }
        renderer.gameObject.SetActive(false);
    }
    void FadeIn()
    {
        foreach (SpriteRenderer sprite in AllRoomSprites)
        {
            if(sprite != null) { StartCoroutine(FadeInSprite(result => sprite.color = result, sprite.color, sprite)); }
            
        }
        foreach(SpriteShapeRenderer shape in AllRoomShapes)
        {
            if (shape != null) { StartCoroutine(FadeInSprite(result => shape.color = result, shape.color, shape)); }
        }
    }
    IEnumerator FadeInSprite(Action<Color> colorToChange, Color baseColor, Renderer renderer)
    {
        renderer.gameObject.SetActive(true);

        float timer = 0;
        while (timer < TransitionTime)
        {
            timer += Time.deltaTime;
            float opacity =  1 / TransitionTime * timer;
            Color newColor = new Color(baseColor.r, baseColor.g, baseColor.b, opacity);
            colorToChange(newColor);
            yield return null;
        }
       
    }

}

