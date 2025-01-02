using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_scriptV2 : MonoBehaviour //DEPRECATED SCPRIPT PLS DELETE
{
    public Action<GameObject> onRoomEntered;
    public Action<GameObject> onRoomExited;

    public Transform ExitPosition;

    public Bounds combinedBounds;
    [SerializeField] Generic_OnTriggerEnterEvents enterRoomCollider;

    Renderer[] childRenderers;
    [SerializeField] bool CalculateBoundsTrigger;
    public bool isBoundPrecalculated;

    [Header("read only")]
    public bool isPlayerInThisRoom;
    public Vector2Int indexInCompleteList;

    //This script exist to hold information of Rooms Generation stuff and Player entering room.
    //No logic of enemies spawned or doors opening or closing should be placed here

    private void Update()
    {
        if (CalculateBoundsTrigger)
        {
            calculateBounds();
            CalculateBoundsTrigger = false;
        }
    }
    private void OnEnable()
    {
        enterRoomCollider.AddActivatorTag(Tags.Player_SinglePointCollider);
        enterRoomCollider.OnTriggerEntered += playerEnteredRoom;
        enterRoomCollider.OnTriggerExited += playerExitedRoom;
    }
    private void OnDisable()
    {
        enterRoomCollider.OnTriggerEntered -= playerEnteredRoom;
        enterRoomCollider.OnTriggerExited -= playerExitedRoom;
    }
    void playerEnteredRoom(Collider2D collision)
    {
        onRoomEntered?.Invoke(collision.gameObject);
        isPlayerInThisRoom = true;
    }
    void playerExitedRoom(Collider2D collision)
    {
        onRoomExited?.Invoke(collision.gameObject);
        isPlayerInThisRoom = false;
    }

    public void calculateBounds()
    {
        childRenderers = GetComponentsInChildren<Renderer>(); //get all renderers

        combinedBounds = new Bounds(transform.position, Vector2.zero); //set the bounds as empty

        foreach (Renderer renderer in childRenderers) //combine all bounds
        {
            combinedBounds.Encapsulate(renderer.bounds);
        }

        //Create the 2Dcollider in the OnTriggerEnter and save the public bounds 
        combinedBounds = UsefullMethods.BoundsToBoxCollider(combinedBounds, transform.position, enterRoomCollider.gameObject).bounds;
    }
}
