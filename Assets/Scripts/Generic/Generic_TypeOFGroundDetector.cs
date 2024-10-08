using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_TypeOFGroundDetector : MonoBehaviour
{
    public Generic_References references; //We have this here so the proximity detector can get the root Transform
    public enum TypesOfGround
    {
        defaultGround, puddle
    }
    public TypesOfGround currentGround = TypesOfGround.defaultGround;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Puddle")) { currentGround = TypesOfGround.puddle; }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Puddle")) { currentGround = TypesOfGround.defaultGround; }
    }
}
