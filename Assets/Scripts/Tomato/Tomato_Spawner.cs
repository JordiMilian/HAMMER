using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato_Spawner : MonoBehaviour
{
    Transform Player;
    [SerializeField] Transform TomatoHandTransform;
    [SerializeField] GameObject TomatoPrefab;
    void Awake()
    {
        Player = GameObject.Find(TagsCollection.MainCharacter).transform;
    }
    public void SpawnTomato()
    {
        //Get direction to Player and move the hand towards it
        Vector3 PlayerPos = (Vector3)Player.position;
        Vector3 DirectionToPlayer = PlayerPos - new Vector3(TomatoHandTransform.position.x, TomatoHandTransform.position.y);
        TomatoHandTransform.up = Vector3.RotateTowards(TomatoHandTransform.up, DirectionToPlayer, 100 * Time.deltaTime, 10);

        //Instantiante the tomato
        var Tomato = Instantiate(TomatoPrefab, TomatoHandTransform.position, TomatoHandTransform.rotation);
    }

}
