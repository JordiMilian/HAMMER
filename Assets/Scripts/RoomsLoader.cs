using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsLoader : MonoBehaviour
{
    [SerializeField] LoadingScreenController loadingScreenController;
    public GameObject CurrentLoadedRoom;
    IRoom currentRoomInterface;
    bool isLoading;
    public IEnumerator LoadNewRoom(GameObject newRoom)
    {
        while (isLoading) { yield return null; } //if we are currently loading a room, wait until we are done

        isLoading = true;
        yield return loadingScreenController.FadeInScreen(); //Fade loading screen

        //unload current if there is one
        if (CurrentLoadedRoom != null)
        {
            currentRoomInterface.OnRoomUnloaded();
            Destroy(CurrentLoadedRoom);
        }

        //load new room
        CurrentLoadedRoom = GameObject.Instantiate(newRoom, transform.position, Quaternion.identity);
        currentRoomInterface = CurrentLoadedRoom.GetComponent<IRoom>();
        currentRoomInterface.OnRoomLoaded();

        StartCoroutine( loadingScreenController.FadeOutScreen()); //Fade out loading screen
        isLoading = false;
        GameEvents.OnLoadNewRoom?.Invoke();
    }
}
