using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class Tomato_Controller : MonoBehaviour
{
    Rigidbody2D Tomato_Rigidbody;
    float Tomato_speed =6;
    public GameObject ArrowSprite;
    public GameObject SplashPrefab;
    RotationConstraint SpritesConstraint;
   
    void Start()
    {
        ConstraintSource CameraConstrain = new ConstraintSource();
        CameraConstrain.sourceTransform = GameObject.Find("Main Camera").transform;
        CameraConstrain.weight = 1;
        SpritesConstraint = GetComponentInChildren<RotationConstraint>();
        SpritesConstraint.AddSource(CameraConstrain);
        Destroy(ArrowSprite);
        Tomato_Rigidbody = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        Tomato_Rigidbody.velocity = transform.up * Tomato_speed;
    }
    public void DestroyTomato()
    {
        var Splash = Instantiate(SplashPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
