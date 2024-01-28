using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Generic_OnTriggerEnterEvents;


public class Rooms_FadInOut : MonoBehaviour
{
    SpriteRenderer[] bottomRoomSprites = new SpriteRenderer[0];
    SpriteRenderer[] topRoomSprites = new SpriteRenderer[0];
    [SerializeField] Generic_OnTriggerEnterEvents topTrigger;
    [SerializeField] Generic_OnTriggerEnterEvents bottomTrigger;
    public GameObject TopRoom;
    public GameObject BottomRoom;
    [SerializeField] float TransitionTime = 0.2f;
    [SerializeField] bool invertStartingRoom;
    private void Awake()
    {
        bottomRoomSprites = BottomRoom.GetComponentsInChildren<SpriteRenderer>();
        topRoomSprites = TopRoom.GetComponentsInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        if (invertStartingRoom) { topTrigger.gameObject.SetActive(false); }
        else { bottomTrigger.gameObject.SetActive(false); }
    }
    private void OnEnable()
    {
        topTrigger.ActivatorTags.Add("Player_SinglePointCollider");
        topTrigger.OnTriggerEntered += playerEnteredTop;

        bottomTrigger.ActivatorTags.Add("Player_SinglePointCollider");
        bottomTrigger.OnTriggerEntered += playerEnteredBottom;
    }
    private void OnDisable()
    {
        topTrigger.OnTriggerEntered -= playerEnteredTop;
        bottomTrigger.OnTriggerEntered -= playerEnteredBottom;
    }
    void playerEnteredTop(object sender, EventArgsTriggererInfo args)
    {
        topTrigger.gameObject.SetActive(false);
        bottomTrigger.gameObject.SetActive(true);
        FadeOut(bottomRoomSprites, BottomRoom);
        FadeIn(topRoomSprites, TopRoom);
    }
    void playerEnteredBottom(object sender, EventArgsTriggererInfo args)
    {
        topTrigger.gameObject.SetActive(true);
        bottomTrigger.gameObject.SetActive(false);
        FadeOut(topRoomSprites, TopRoom);
        FadeIn(bottomRoomSprites, BottomRoom);
    }

    void FadeOut(SpriteRenderer[] sprites, GameObject Room)
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            StartCoroutine(FadeOutSprite(sprite,Room));
        }
    }
    IEnumerator FadeOutSprite(SpriteRenderer sprite, GameObject room)
    {
        float timer = 0;
        while(timer < TransitionTime)
        {
            timer += Time.deltaTime;
            float opacity = 1- (1 / TransitionTime * timer);
            sprite.color = new Color (1,1,1,opacity);
            yield return null;
        }
        room.SetActive(false);
    }

    void FadeIn(SpriteRenderer[] sprites, GameObject Room)
    {
        Room.SetActive(true);
        foreach (SpriteRenderer sprite in sprites)
        {
            StartCoroutine(FadeInSprites(sprite, Room));
        }
    }
    IEnumerator FadeInSprites(SpriteRenderer sprite, GameObject room)
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
