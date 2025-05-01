using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms_FadeInOut_StartingRoomsCheck : MonoBehaviour
{
    public static Rooms_FadeInOut_StartingRoomsCheck Instance;
    public List<Rooms_FadeInOut> roomsFades;
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
        foreach (Rooms_FadeInOut fadeIn in roomsFades)
        {
            Debug.Log("Faded " + fadeIn.transform.parent.gameObject.name);
            fadeIn.checkCurrentRoom(); //es podrie fer mes limpio pero suda
            
        }
    }
}
