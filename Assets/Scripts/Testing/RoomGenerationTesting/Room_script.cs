using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room_script : MonoBehaviour
{
    public Action<GameObject, Room_script> onRoomEntered;
    public Action<GameObject, Room_script> onRoomExited;

    public Transform ExitPosition;

    //public Bounds combinedBounds;
    public Generic_OnTriggerEnterEvents enterRoomCollider;

    
    [SerializeField] bool CalculateBoundsTrigger;
    public bool isBoundPrecalculated;

    [Header("read only")]
    public bool isPlayerInThisRoom;
    public Vector2Int indexInCompleteList;
    [SerializeField] Transform GroundRenderersRoot;

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
        enterRoomCollider.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
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
        onRoomEntered?.Invoke(collision.gameObject, this);
        isPlayerInThisRoom = true;
    }
    void playerExitedRoom(Collider2D collision)
    {
        onRoomExited?.Invoke(collision.gameObject, this);
        isPlayerInThisRoom = false;
    }

    public void calculateBounds()
    {
        Renderer[] childRenderers = GroundRenderersRoot.GetComponentsInChildren<Renderer>(); //get all renderers

        Bounds combinedBounds = new Bounds(transform.position, Vector2.zero); //set the bounds as empty

        foreach (Renderer renderer in childRenderers) //combine all bounds
        {
            combinedBounds.Encapsulate(renderer.bounds);
        }

        //Create the 2Dcollider in the OnTriggerEnter and save the public bounds 
        UsefullMethods.BoundsToBoxCollider(combinedBounds, transform.position, enterRoomCollider.gameObject);
    }
}
