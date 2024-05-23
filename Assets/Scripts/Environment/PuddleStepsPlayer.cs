using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.VFX;

public class PuddleStepsPlayer : MonoBehaviour
{
    [SerializeField] VisualEffect stepEffect;
    [SerializeField] float MinSecondsPerStep;
    [SerializeField] float MaxEntitySpeedPerSecond;

    [SerializeField] float regularStepIntensity;
    public bool isStepping;
    bool startStepping;
    public Transform followingEntityTf;
    Vector2 lastPos;
    float timer;
    private void Start()
    {
        startStepping = true;
        //maxSqrSpeedPerSecond = Mathf.Pow(MaxEntitySpeedPerSecond, 2);
        //InvokeRepeating("playAStep", 0, MaxSecondsPerStep);
    }
    private void Update()
    {
        if (!isStepping) { return; }

        if (startStepping) 
        { 
            lastPos = followingEntityTf.position; 
            startStepping = false;
            return;
        }
        Vector2 nowPos = followingEntityTf.position;
        float distanceThisFrame = (nowPos - lastPos).magnitude;
        lastPos = nowPos;
        float currentSpeedPerSeconds = distanceThisFrame / Time.deltaTime;
        float normalizedSpeed = Mathf.InverseLerp(0,MaxEntitySpeedPerSecond, currentSpeedPerSeconds);
        float currentSecondsPerStep = Mathf.Lerp( 2,MinSecondsPerStep, normalizedSpeed);
        //Debug.Log("Current speed per seconds: " + currentSpeedPerSeconds);
        timer += Time.deltaTime;
        if(timer > currentSecondsPerStep)
        {
            timer = 0;
            playAStep();
        }
    }
    void playAStep()
    {
        stepEffect.gameObject.transform.position = lastPos;
        stepEffect.SetFloat("Intensity", regularStepIntensity);
        stepEffect.Play();
    }
}
