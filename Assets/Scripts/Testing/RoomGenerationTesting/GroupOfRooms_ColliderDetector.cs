using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupOfRooms_ColliderDetector : MonoBehaviour
{
    public Action<GameObject> OnPLayerEnteredGroup;
    public Action<GameObject> OnPLayerExitedGroup;
    private void OnEnable()
    {
        //OnPLayerEnteredGroup += CurrentPlayersRoomManager.Instance.SetCurrentGroup;
    }
    private void OnDisable()
    {
        //OnPLayerEnteredGroup -= CurrentPlayersRoomManager.Instance.SetCurrentGroup;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            OnPLayerEnteredGroup?.Invoke(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            OnPLayerExitedGroup?.Invoke(gameObject);
        }
    }
}
