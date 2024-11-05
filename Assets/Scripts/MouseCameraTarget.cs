using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCameraTarget : MonoBehaviour
{
    [SerializeField] float controllerDistanceMultiplier;
    InputDetector inputDetector;
    Vector2 lastValidDirection;
    Vector2 lerpedDirection;
    Coroutine currentReturnToCenter;
    bool coroutineStarted;
    public static MouseCameraTarget Instance;
    private void Awake()
    {
        inputDetector = InputDetector.Instance;

        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroyed here:" + gameObject.name);
            Destroy(this);
        }
        else
        {
            Debug.Log("Singleton Here: " + gameObject.name);
            Instance = this;
        }
    }
    private void Update()
    {
        if (!inputDetector.isControllerDetected) { transform.position = inputDetector.MousePosition; return; } //If KEYBOARD, just go to mouse always and return

        //If CONTROLLER
        if(inputDetector.LookingDirectionInput.sqrMagnitude > .7f) //If given some axis input, just look there
        {
            if(coroutineStarted)
            {
                stopReturnToCenter();
            }
            lastValidDirection = inputDetector.LookingDirectionInput;
            transform.position = inputDetector.PlayerPos + (inputDetector.LookingDirectionInput * controllerDistanceMultiplier);
        }
        else //When axis input is insuficient, slowly return to center
        {
            if(!coroutineStarted)
            {
                restartReturnToCenter();
            }
            transform.position = inputDetector.PlayerPos + (lerpedDirection * controllerDistanceMultiplier);
        }

        
    }
    void restartReturnToCenter()
    {
        if (currentReturnToCenter != null) { StopCoroutine(currentReturnToCenter); }
        currentReturnToCenter = StartCoroutine(slowlyReturnToCenter(lastValidDirection, 15));
        coroutineStarted = true;
    }
    void stopReturnToCenter()
    {
        if (currentReturnToCenter != null) { StopCoroutine(currentReturnToCenter); }
        coroutineStarted = false;
    }
    IEnumerator slowlyReturnToCenter(Vector2 initialDirection, float duration)
    {
        float timer = 0;
        lerpedDirection = initialDirection;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            lerpedDirection = Vector2.Lerp(initialDirection, Vector2.zero, timer/duration);
            yield return null;
        }
        lerpedDirection = Vector2.zero;
    }
}
