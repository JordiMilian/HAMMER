using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GesturesDetector : MonoBehaviour
{
    const float secondsOfInputStored = .3f;
    [Serializable]
    public struct InputValue
    {
        public Vector2 Input;
        public float time;
    }
    List<InputValue> lastInputValues = new List<InputValue>();
    private void Update()
    {
        FillAndDrawInput();

        if (CheckForArc(out Vector2 arcDirection, out bool isClockWise))
        {
            StartCoroutine(DrawArc(arcDirection, isClockWise, 1));
            Debug.Log("Arc detected");
        }
        else if (CheckForTap(out Vector2 tapDirection))
        { 
            StartCoroutine(DrawTap(tapDirection, 1));
            Debug.Log("Tap detected");
        }
       
    }
    #region GET INPUTS
    void FillAndDrawInput()
    {
        Vector2 thisFrameInput = Input.GetAxis("Horizontal") * Vector2.right + Input.GetAxis("Vertical") * Vector2.up; //I dont like this, its not normalized input

        InputValue inputValue = new InputValue();
        inputValue.Input = thisFrameInput;
        inputValue.time = Time.time;
        lastInputValues.Add(inputValue);

        for (int i = lastInputValues.Count - 1; i >= 0; i--)
        {
            if (happenedBeforeSeconds(lastInputValues[i], secondsOfInputStored))
            {
                lastInputValues.RemoveAt(i);
            }
        }
        for (int i = 0; i < lastInputValues.Count; i++)
        {
            if (i == lastInputValues.Count - 1) { Debug.DrawLine(transform.position, (Vector2)transform.position + lastInputValues[i].Input, Color.green); }
            else
            {
                Debug.DrawLine((Vector2)transform.position + lastInputValues[i].Input, (Vector2)transform.position + lastInputValues[i + 1].Input, Color.red);
            }
        }
    }
    #endregion
    #region ARC CHECK
    const int minInputsToCheckForArc = 5; //if there are not this many inputs stored in the list, do not check. This is for the beggining of play
    const float minAngleDegForArc = 45f;
    const float secondsToCheckForArc = 0.2f; //check values that happenend this many seconds before
    const float delayAfterArc = 0.3f; //after a true arc, delay the checks so we have new values
    const float thresholdForArc = 0.2f; //If an input vector below this magnitude is found, stop the check
    bool isDelayingArc;
     bool CheckForArc(out Vector2 arcDirection, out bool isClockWise)
    {
        arcDirection = Vector2.zero;
        isClockWise = false;

        if (isDelayingArc) { return false; }
        if (lastInputValues.Count < minInputsToCheckForArc) { return false; } //if we dont have enough inputs, return false


        float totalAngleDeg = 0;
        for (int i = lastInputValues.Count -1; i > 0; i--) //Go throw all the values and add up all their angles. If the add up to a minim, return true
        {
            InputValue thisInputValue = lastInputValues[i];
            InputValue previousValue = lastInputValues[i - 1];

            if (isBelowThreshold(thisInputValue,thresholdForArc)) { return false; }
            if (happenedBeforeSeconds(thisInputValue, secondsToCheckForArc)) { return false; }

            float DegBetweenValues = UsefullMethods.AngleBetweenDirectionsDeg(thisInputValue.Input, previousValue.Input);
            totalAngleDeg += DegBetweenValues;

            if(Mathf.Abs(totalAngleDeg) >= minAngleDegForArc)
            {
                isClockWise = totalAngleDeg > 0;
                arcDirection = lastInputValues[lastInputValues.Count - 2].Input.normalized; //this needs a better calculation, check for medians somehow doesnt work
                StartCoroutine(ArcDelay(arcDirection));
                return true;
            }
        }
        return false;
    }
    Vector2 GetMedianVectorOfInputs(int lastInputIndex)
    {
        Vector2 medianVector = Vector2.zero;
        for (int i = lastInputValues.Count -1; i >= lastInputIndex; i--)
        {
            medianVector += lastInputValues[i].Input;
        }
        return medianVector.normalized;
    }
    IEnumerator DrawArc(Vector2 arcDirection, bool isClockWise, float duration)
    {
        float timer = 0;
        Color directionColor = isClockWise ? Color.red : Color.green;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            //Handles.DrawWireArc(transform.position, Vector3.forward, arcDirection, minAngleDegForArc, 1.5f);
            Debug.DrawLine(transform.position, (Vector2)transform.position + arcDirection, directionColor);
            yield return null;
        }
    }
    IEnumerator ArcDelay(Vector2 arcDirection)
    {
        isDelayingArc = true;
        yield return new WaitForSeconds(delayAfterArc);
        isDelayingArc = false;
    }

    #endregion
    #region TAP CHECK
    const float thresholdForTap = 0.5f; //The active input value needs to be above this magnitude to be considered a tap
    const float secondsToCheckForTaps = 0.1f; //Check inputs that happened this many seconds before this frame
    const float delayAfterTap = 0.2f; //After an active Tap, delay so we have new values
    bool isDelayingTapCheck;
    public bool CheckForTap(out Vector2 tapDirection)
    {
        tapDirection = Vector2.zero;
        if (isDelayingTapCheck) { return false; }
        if(lastInputValues.Count < 4) { return false; }

        if (!isBelowThreshold(lastInputValues[lastInputValues.Count - 1],thresholdForTap)) { return false; } //if last input is active, return false

        for (int i = lastInputValues.Count -1; i >= 0; i--) //go thorw all the inputs to check for a positive input, if we do, check for a negative input right after
        {
            InputValue inputValue = lastInputValues[i];
            tapDirection = inputValue.Input.normalized;
            if (happenedBeforeSeconds(inputValue,secondsToCheckForTaps)) { return false; } //if the input is too old, break

            if (!isBelowThreshold(inputValue,thresholdForTap))
            {
                for (int j = i; j >= 0; j--)
                {
                    if (isBelowThreshold(lastInputValues[j], thresholdForTap)) { StartCoroutine(TapDelay(tapDirection)); return true; }
                }
            }
        }
        return false;
    }
    IEnumerator TapDelay(Vector2 tapDirection)
    {
        isDelayingTapCheck = true;
        yield return new WaitForSeconds(delayAfterTap);
        isDelayingTapCheck = false;
    }
    IEnumerator DrawTap(Vector2 tapDirection, float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            Debug.DrawLine(transform.position, (Vector2)transform.position + tapDirection, Color.blue);
            yield return null;
        }
    }
    bool isBelowThreshold(InputValue inputValue, float threshold)
    {
        return inputValue.Input.sqrMagnitude < threshold;
    }
    bool happenedBeforeSeconds(InputValue inputValue, float secondsBefore)
    {
        return inputValue.time < (Time.time - secondsBefore);
    }
#endregion
}
