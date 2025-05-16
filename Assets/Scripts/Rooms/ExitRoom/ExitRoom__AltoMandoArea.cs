using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoom__AltoMandoArea : MonoBehaviour
{
    [SerializeField] List<GameObject> RoomsInThisArea = new();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Player_SinglePointCollider))
        {
            GameController.Instance.OnEnteredNewAreaDoor(RoomsInThisArea);
        }
    }


}
