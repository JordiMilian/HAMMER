using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms_FadeInOut_StartingRoomsCheck : MonoBehaviour
{
    public static Rooms_FadeInOut_StartingRoomsCheck Instance;
    public List<Rooms_FadInOut> roomsFades;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void FadeInStartingRoom() //called from sceneStarter
    {
        foreach (Rooms_FadInOut fadeIn in roomsFades)
        {
            fadeIn.checkCurrentRoom(); //es podrie fer mes limpio pero suda
        }
    }
}
