using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room_script : MonoBehaviour
{
    public Transform ExitPosition;
    [HideInInspector]public Bounds combinedWorldBounds;
    Renderer[] childRenderers;
    [SerializeField] bool CalculateBoundsTrigger;
    public bool isBoundCalculated;
    [Header("read only")]
    public bool isPlayerInThisRoom;
    public Action<Vector2Int> OnPlayerEnteredRoom;
    public Action<Vector2Int> OnPlayerExitedRoom;
    public Vector2Int indexInCompleteList;
    private void Update()
    {
        if(CalculateBoundsTrigger)
        {
            calculateBounds();
            CalculateBoundsTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            Debug.Log("player entered");
            OnPlayerEnteredRoom?.Invoke(indexInCompleteList);
            isPlayerInThisRoom = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            OnPlayerExitedRoom?.Invoke(indexInCompleteList);
            isPlayerInThisRoom = false;
        }
    }
    public void calculateBounds()
    {
        childRenderers = GetComponentsInChildren<Renderer>(); //get all renderers

        combinedWorldBounds = new Bounds(transform.position, Vector2.zero); //set the bounds as empty

        foreach (Renderer renderer in childRenderers)
        {
            combinedWorldBounds.Encapsulate(renderer.bounds);
        }
        UsefullMethods.BoundsToBoxCollider(combinedWorldBounds, transform.position, gameObject);
    }
}
