using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using static Generic_OnTriggerEnterEvents;

public class Tomato_Controller : MonoBehaviour
{
    Rigidbody2D Tomato_Rigidbody;
    float Tomato_speed =6;
    public GameObject SplashPrefab;
    RotationConstraint SpritesConstraint;
    [SerializeField] Generic_OnTriggerEnterEvents TomatoTrigger;

    private void Awake()
    {
        ConstraintSource CameraConstrain = new ConstraintSource();
        CameraConstrain.sourceTransform = Camera.main.transform;
        CameraConstrain.weight = 1;
        SpritesConstraint = GetComponentInChildren<RotationConstraint>();
        SpritesConstraint.AddSource(CameraConstrain);
        Tomato_Rigidbody = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        TomatoTrigger.AddActivatorTag(TagsCollection.Instance.Player);
        TomatoTrigger.OnTriggerEntered += TomatoCrashed;
    }
    private void OnDisable()
    {
        TomatoTrigger.OnTriggerEntered -= TomatoCrashed;
    }
    
    void Update()
    {
        Tomato_Rigidbody.velocity = transform.up * Tomato_speed;
    }
    public void TomatoCrashed(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo args)
    {
        var Splash = Instantiate(SplashPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    public void EV_TomatoCrashed()
    { 
        TomatoCrashed(this, new EventArgsCollisionInfo( new Collider2D()));
    }
}
