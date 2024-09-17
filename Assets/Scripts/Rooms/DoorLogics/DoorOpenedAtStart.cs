using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenedAtStart : MonoBehaviour
{
    [SerializeField] DoorAnimationController door;
    private void Start()
    {
        door.InstaOpen();
    }
}
