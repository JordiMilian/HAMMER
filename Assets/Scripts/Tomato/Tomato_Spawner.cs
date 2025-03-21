using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato_Spawner : MonoBehaviour
{
    Transform Player;
    [SerializeField] Transform TomatoHandTransform;
    [SerializeField] GameObject TomatoPrefab;
    [SerializeField] GameObject DestinationGO;
    [SerializeField] float MaxDistance;
    [SerializeField] AudioClip ThrowTomatoSFX;
    void Awake()
    {
        Player = GlobalPlayerReferences.Instance.playerTf;
    }

    public void EV_newSpawnTomato()
    {
        Vector2 PlayerPos = Player.position;
        Vector2 destination = PlayerPos;
        Vector2 HandPosition = TomatoHandTransform.position;

        //If the player is too far edit destination
        if( (PlayerPos - HandPosition).magnitude > MaxDistance)
        {
            Vector2 playerDirection = ( PlayerPos - HandPosition).normalized;
            destination = HandPosition + (playerDirection * MaxDistance);
        }

        GameObject newTomato = Instantiate(TomatoPrefab,TomatoHandTransform.position,Quaternion.identity);
        GameObject newDestination = Instantiate(DestinationGO, destination, Quaternion.identity);

        newTomato.GetComponent<Tomato_NewController>().ThrowItself(newDestination, HandPosition, destination);

        SFX_PlayerSingleton.Instance.playSFX(ThrowTomatoSFX);
    }

}
